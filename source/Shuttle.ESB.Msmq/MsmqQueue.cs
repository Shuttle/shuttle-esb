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
	public class MsmqQueue : IQueue, ICreate, IDrop, IPurge, ICount, IQueueReader, IAcknowledge
	{
		private readonly TimeSpan _timeout;
		private readonly MsmqUriParser parser;
		private readonly MessagePropertyFilter _messagePropertyFilter;
		private readonly Type _msmqDequeuePipelineType = typeof(MsmqDequeuePipeline);
		private readonly ReusableObjectPool<MsmqDequeuePipeline> _dequeuePipelinePool;

		private readonly ILog _log;

		public MsmqQueue(Uri uri, IMsmqConfiguration configuration)
		{
			Guard.AgainstNull(uri, "uri");
			Guard.AgainstNull(configuration, "configuration");

			_log = Log.For(this);

			parser = new MsmqUriParser(uri);

			_timeout = parser.Local
				           ? TimeSpan.FromMilliseconds(configuration.LocalQueueTimeoutMilliseconds)
				           : TimeSpan.FromMilliseconds(configuration.RemoteQueueTimeoutMilliseconds);

			Uri = parser.Uri;

			_messagePropertyFilter = new MessagePropertyFilter();
			_messagePropertyFilter.SetAll();

			_dequeuePipelinePool = new ReusableObjectPool<MsmqDequeuePipeline>();
		}

		public int Count
		{
			get
			{
				var count = 0;

				using (var queue = CreateQueue())
				using (var enumerator = queue.GetMessageEnumerator2())
				{
					while (enumerator.MoveNext(new TimeSpan(0, 0, 0)))
					{
						count++;
					}
				}

				return count;
			}
		}

		public void Create()
		{
			CreateJournal();

			if (Exists())
			{
				return;
			}

			if (!parser.Local)
			{
				throw new InvalidOperationException(string.Format(MsmqResources.CannotCreateRemoteQueue, Uri));
			}

			MessageQueue.Create(parser.Path, parser.Transactional).Dispose();

			_log.Information(string.Format(MsmqResources.QueueCreated, parser.Path));
		}

		private void CreateJournal()
		{
			if (!parser.Journal)
			{
				return;
			}

			if (JournalExists())
			{
				return;
			}

			if (!parser.Local)
			{
				throw new InvalidOperationException(string.Format(MsmqResources.CannotCreateRemoteQueue, Uri));
			}

			MessageQueue.Create(parser.JournalPath, parser.Transactional).Dispose();

			_log.Information(string.Format(MsmqResources.QueueCreated, parser.Path));
		}

		public void Drop()
		{
			DropJournal();

			if (!Exists())
			{
				return;
			}

			if (!parser.Local)
			{
				throw new InvalidOperationException(string.Format(MsmqResources.CannotDropRemoteQueue, Uri));
			}

			MessageQueue.Delete(parser.Path);

			_log.Information(string.Format(MsmqResources.QueueDropped, Uri));
		}

		private void DropJournal()
		{
			if (!JournalExists())
			{
				return;
			}

			if (!parser.Local)
			{
				throw new InvalidOperationException(string.Format(MsmqResources.CannotDropRemoteQueue, Uri));
			}

			MessageQueue.Delete(parser.JournalPath);

			_log.Information(string.Format(MsmqResources.QueueDropped, Uri));
		}

		public void Purge()
		{
			using (var queue = CreateQueue())
			{
				queue.Purge();
			}

			_log.Information(string.Format(MsmqResources.QueuePurged, Uri));
		}

		public Uri Uri { get; private set; }

		private bool Exists()
		{
			return MessageQueue.Exists(parser.Path);
		}

		private bool JournalExists()
		{
			return MessageQueue.Exists(parser.JournalPath);
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
					AccessDenied();
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
					queue.Send(sendMessage, TransactionType());
				}
			}
			catch (MessageQueueException ex)
			{
				if (ex.MessageQueueErrorCode == MessageQueueErrorCode.AccessDenied)
				{
					AccessDenied();
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

		public Stream Dequeue()
		{
			try
			{
				var pipeline = _dequeuePipelinePool.Get(_msmqDequeuePipelineType) ?? new MsmqDequeuePipeline();

				pipeline.Execute(parser, _timeout);

				_dequeuePipelinePool.Release(pipeline);

				var message = pipeline.State.Get<Message>();

				return message == null ? null : message.BodyStream;
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(MsmqResources.DequeueError, Uri, ex.CompactMessages()));

				throw;
			}
		}

		public Stream Dequeue(Guid messageId)
		{
			try
			{
				Message message;

				using (var queue = CreateQueue())
				{
					message = queue.ReceiveByCorrelationId(string.Format(@"{0}\1", messageId), TransactionType());
				}

				if (message == null)
				{
					return null;
				}

				return message.BodyStream;
			}
			catch (MessageQueueException ex)
			{
				if (ex.MessageQueueErrorCode == MessageQueueErrorCode.AccessDenied)
				{
					AccessDenied();
				}

				_log.Error(string.Format(MsmqResources.DequeueMessageIdError, messageId, Uri, ex.CompactMessages()));

				throw;
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(MsmqResources.DequeueMessageIdError, messageId, Uri, ex.CompactMessages()));

				throw;
			}
		}

		public bool Remove(Guid messageId)
		{
			try
			{
				using (var queue = CreateQueue())
				{
					return (queue.ReceiveByCorrelationId(string.Format(@"{0}\1", messageId), TransactionType()) != null);
				}
			}
			catch (MessageQueueException ex)
			{
				if (ex.MessageQueueErrorCode == MessageQueueErrorCode.AccessDenied)
				{
					AccessDenied();
				}

				_log.Error(string.Format(MsmqResources.RemoveError, messageId, Uri, ex.CompactMessages()));

				throw;
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(MsmqResources.RemoveError, messageId, Uri, ex.CompactMessages()));

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
					AccessDenied();
				}

				_log.Error(string.Format(MsmqResources.ReadError, top, Uri, ex.CompactMessages()));
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(MsmqResources.ReadError, top, Uri, ex.CompactMessages()));
			}

			return result;
		}

		private MessageQueue CreateQueue()
		{
			return new MessageQueue(parser.Path)
				{
					MessageReadPropertyFilter = _messagePropertyFilter
				};
		}

		private MessageQueue CreateJournalQueue()
		{
			return new MessageQueue(parser.JournalPath)
				{
					MessageReadPropertyFilter = _messagePropertyFilter
				};
		}

		private void AccessDenied()
		{
			_log.Fatal(
				string.Format(
					MsmqResources.AccessDenied,
					WindowsIdentity.GetCurrent() != null
						? WindowsIdentity.GetCurrent().Name
						: MsmqResources.Unknown,
					parser.Path));

			if (Environment.UserInteractive)
			{
				return;
			}

			Process.GetCurrentProcess().Kill();
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

		private MessageQueueTransactionType TransactionType()
		{
			return parser.Transactional
				       ? InTransactionScope
					         ? MessageQueueTransactionType.Automatic
					         : parser.Transactional
						           ? MessageQueueTransactionType.Single
						           : MessageQueueTransactionType.None
				       : MessageQueueTransactionType.None;
		}

		private static bool InTransactionScope
		{
			get { return Transaction.Current != null; }
		}

		public void Acknowledge(Guid messageId)
		{
			if (!parser.Journal)
			{
				return;
			}

			try
			{
				using (var queue = CreateJournalQueue())
				{
					queue.ReceiveByCorrelationId(string.Format(@"{0}\1", messageId), TransactionType());
				}
			}
			catch (MessageQueueException ex)
			{
				if (ex.MessageQueueErrorCode == MessageQueueErrorCode.AccessDenied)
				{
					AccessDenied();
				}

				_log.Error(string.Format(MsmqResources.RemoveError, messageId, Uri, ex.CompactMessages()));

				throw;
			}
			catch (Exception ex)
			{
				_log.Error(string.Format(MsmqResources.RemoveError, messageId, Uri, ex.CompactMessages()));

				throw;
			}
		}
	}
}