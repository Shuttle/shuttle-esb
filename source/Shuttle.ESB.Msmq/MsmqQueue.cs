using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Messaging;
using System.Security.Principal;
using System.Transactions;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Msmq
{
	public class MsmqQueue : IQueue, ICreate, IDrop, IPurge, IQueueReader, IRequireInitialization
	{
		private readonly TimeSpan _timeout;
		private readonly MsmqUriParser _parser;
		private readonly MessagePropertyFilter _messagePropertyFilter;
		private readonly Type _msmqDequeuePipelineType = typeof(MsmqGetMessagePipeline);
		private readonly ReusableObjectPool<MsmqGetMessagePipeline> _dequeuePipelinePool;
		private readonly object _padlock = new object();
		private bool _journalMessagesReturned;

		private readonly ILog _log;

		public MsmqQueue(Uri uri, IMsmqConfiguration configuration)
		{
			Guard.AgainstNull(uri, "uri");
			Guard.AgainstNull(configuration, "configuration");

			_log = Log.For(this);

			_parser = new MsmqUriParser(uri);

			_timeout = _parser.Local
						   ? TimeSpan.FromMilliseconds(configuration.LocalQueueTimeoutMilliseconds)
						   : TimeSpan.FromMilliseconds(configuration.RemoteQueueTimeoutMilliseconds);

			Uri = _parser.Uri;

			_messagePropertyFilter = new MessagePropertyFilter();
			_messagePropertyFilter.SetAll();

			_dequeuePipelinePool = new ReusableObjectPool<MsmqGetMessagePipeline>();
		}

		private void ReturnJournalMessages()
		{
			lock (_padlock)
			{
				if (_journalMessagesReturned
					||
					!_parser.Journal
					||
					!Exists()
					||
					!JournalExists())
				{
					return;
				}

				new MsmqReturnJournalPipeline().Execute(_parser, _timeout);

				_journalMessagesReturned = true;
			}
		}

		public void Create()
		{
			CreateJournal();

			if (Exists())
			{
				return;
			}

			if (!_parser.Local)
			{
				throw new InvalidOperationException(string.Format(MsmqResources.CannotCreateRemoteQueue, Uri));
			}

			MessageQueue.Create(_parser.Path, _parser.Transactional).Dispose();

			_log.Information(string.Format(MsmqResources.QueueCreated, _parser.Path));
		}

		private void CreateJournal()
		{
			if (!_parser.Journal)
			{
				return;
			}

			if (JournalExists())
			{
				return;
			}

			if (!_parser.Local)
			{
				throw new InvalidOperationException(string.Format(MsmqResources.CannotCreateRemoteQueue, Uri));
			}

			MessageQueue.Create(_parser.JournalPath, _parser.Transactional).Dispose();

			_log.Information(string.Format(MsmqResources.QueueCreated, _parser.Path));
		}

		public void Drop()
		{
			DropJournal();

			if (!Exists())
			{
				return;
			}

			if (!_parser.Local)
			{
				throw new InvalidOperationException(string.Format(MsmqResources.CannotDropRemoteQueue, Uri));
			}

			MessageQueue.Delete(_parser.Path);

			_log.Information(string.Format(MsmqResources.QueueDropped, _parser.Path));
		}

		private void DropJournal()
		{
			if (!JournalExists())
			{
				return;
			}

			if (!_parser.Local)
			{
				throw new InvalidOperationException(string.Format(MsmqResources.CannotDropRemoteQueue, Uri));
			}

			MessageQueue.Delete(_parser.JournalPath);

			_log.Information(string.Format(MsmqResources.QueueDropped, _parser.JournalPath));
		}

		public void Purge()
		{
			using (var queue = CreateQueue())
			{
				queue.Purge();
			}

			_log.Information(string.Format(MsmqResources.QueuePurged, _parser.Path));
		}

		public Uri Uri { get; private set; }

		private bool Exists()
		{
			return MessageQueue.Exists(_parser.Path);
		}

		private bool JournalExists()
		{
			return MessageQueue.Exists(_parser.JournalPath);
		}

		public bool IsEmpty()
		{
			try
			{
				using (var queue = CreateQueue())
				{
					return queue.Peek(_timeout) == null;
				}
			}
			catch (MessageQueueException ex)
			{
				if (ex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
				{
					return true;
				}

				if (ex.MessageQueueErrorCode == MessageQueueErrorCode.AccessDenied)
				{
					AccessDenied(_log, _parser.Path);
				}

				throw;
			}
		}

		public void Enqueue(Guid messageId, Stream stream)
		{
			var sendMessage = new Message
				{
					Recoverable = true,
					Label = messageId.ToString(),
					CorrelationId = string.Format(@"{0}\1", messageId),
					BodyStream = stream
				};

			try
			{
				using (var queue = CreateQueue())
				{
					queue.Send(sendMessage, TransactionType(_parser.Transactional));
				}
			}
			catch (MessageQueueException ex)
			{
				if (ex.MessageQueueErrorCode == MessageQueueErrorCode.AccessDenied)
				{
					AccessDenied(_log, _parser.Path);
				}

				_log.Error(string.Format(MsmqResources.SendMessageIdError, messageId, Uri, ex.CompactMessages()));

				throw;
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(MsmqResources.SendMessageIdError, messageId, Uri, ex.CompactMessages()));

				throw;
			}
		}

		public ReceivedMessage GetMessage()
		{
			if (!_journalMessagesReturned)
			{
				ReturnJournalMessages();
			}

			try
			{
				var pipeline = _dequeuePipelinePool.Get(_msmqDequeuePipelineType) ?? new MsmqGetMessagePipeline();

				pipeline.Execute(_parser, _timeout);

				_dequeuePipelinePool.Release(pipeline);

				var message = pipeline.State.Get<Message>();

				return message == null ? null : new ReceivedMessage(message.BodyStream, new Guid(message.Label));
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(MsmqResources.GetMessageError, _parser.Path, ex.CompactMessages()));

				throw;
			}
		}

		public IEnumerable<Stream> Read(int top)
		{
			var result = new List<Stream>();

			var count = 0;

			try
			{
				using (var queue = CreateQueue())
				using (var cursor = queue.CreateCursor())
				{
					var peek = PeekMessage(queue, cursor, PeekAction.Current);

					while (peek != null && (top == 0 || count < top))
					{
						result.Add(peek.BodyStream);

						count++;

						peek = PeekMessage(queue, cursor, PeekAction.Next);
					}
				}
			}
			catch (MessageQueueException ex)
			{
				if (ex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
				{
					return null;
				}

				if (ex.MessageQueueErrorCode == MessageQueueErrorCode.AccessDenied)
				{
					AccessDenied(_log, _parser.Path);
				}

				_log.Error(string.Format(MsmqResources.ReadError, top, _parser.Path, ex.CompactMessages()));
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(MsmqResources.ReadError, top, _parser.Path, ex.CompactMessages()));
			}

			return result;
		}

		private MessageQueue CreateQueue()
		{
			return new MessageQueue(_parser.Path)
				{
					MessageReadPropertyFilter = _messagePropertyFilter
				};
		}

		private MessageQueue CreateJournalQueue()
		{
			return new MessageQueue(_parser.JournalPath)
				{
					MessageReadPropertyFilter = _messagePropertyFilter
				};
		}

		private Message PeekMessage(MessageQueue queue, Cursor cursor, PeekAction action)
		{
			try
			{
				return queue.Peek(_timeout, cursor, action);
			}
			catch
			{
				return null;
			}
		}

		public static MessageQueueTransactionType TransactionType(bool transactional)
		{
			return transactional
					   ? InTransactionScope
							 ? MessageQueueTransactionType.Automatic
							 : transactional
								   ? MessageQueueTransactionType.Single
								   : MessageQueueTransactionType.None
					   : MessageQueueTransactionType.None;
		}

		private static bool InTransactionScope
		{
			get { return Transaction.Current != null; }
		}

		public void Acknowledge(object acknowledgementToken)
		{
			if (!_parser.Journal)
			{
				return;
			}

			var messageId = (Guid)acknowledgementToken;

			try
			{
				using (var queue = CreateJournalQueue())
				{
					queue.ReceiveByCorrelationId(string.Format(@"{0}\1", messageId), TransactionType(_parser.Transactional));
				}
			}
			catch (MessageQueueException ex)
			{
				if (ex.MessageQueueErrorCode == MessageQueueErrorCode.AccessDenied)
				{
					AccessDenied(_log, _parser.Path);
				}

				_log.Error(string.Format(MsmqResources.RemoveError, messageId, _parser.Path, ex.CompactMessages()));

				throw;
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(MsmqResources.RemoveError, messageId, _parser.Path, ex.CompactMessages()));

				throw;
			}
		}

		public void Release(object acknowledgementToken)
		{
			if (!_parser.Journal
				||
				!Exists()
				||
				!JournalExists())
			{
				return;
			}

			new MsmqReleaseMessagePipeline().Execute((Guid)acknowledgementToken, _parser, _timeout);
		}

		public static void AccessDenied(ILog log, string path)
		{
			Guard.AgainstNull(log, "log");
			Guard.AgainstNull(path, "path");

			log.Fatal(
				string.Format(
					MsmqResources.AccessDenied,
					WindowsIdentity.GetCurrent() != null
						? WindowsIdentity.GetCurrent().Name
						: MsmqResources.Unknown,
					path));

			if (Environment.UserInteractive)
			{
				return;
			}

			Process.GetCurrentProcess().Kill();
		}

		public void Initialize(IServiceBus bus)
		{
			ReturnJournalMessages();
		}
	}
}