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
	public class MsmqQueue : IQueue, ICreate, IDrop, IPurge, ICount, IQueueReader
	{
		private readonly TimeSpan timeout;

		private readonly ILog log;

		private readonly MsmqUriParser parser;

		public MsmqQueue(Uri uri, IMsmqConfiguration configuration)
		{
			Guard.AgainstNull(uri, "uri");
			Guard.AgainstNull(configuration, "configuration");

			log = Log.For(this);

			parser = new MsmqUriParser(uri);

			timeout = parser.Local
				          ? TimeSpan.FromMilliseconds(configuration.LocalQueueTimeoutMilliseconds )
				          : TimeSpan.FromMilliseconds(configuration.RemoteQueueTimeoutMilliseconds);

			Uri = parser.Uri;
		}

		public int Count
		{
			get
			{
				var count = 0;

				using (var queue = CreateGuardedQueue())
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
			if (Exists())
			{
				return;
			}

			if (!parser.Local)
			{
				throw new InvalidOperationException(string.Format(MsmqResources.CannotCreateRemoteQueue, Uri));
			}

			MessageQueue.Create(parser.Path, parser.Transactional).Dispose();

			log.Information(string.Format(MsmqResources.QueueCreated, Uri));
		}

		public void Drop()
		{
			if (!Exists())
			{
				return;
			}

			if (!parser.Local)
			{
				throw new InvalidOperationException(string.Format(MsmqResources.CannotDropRemoteQueue, Uri));
			}

			MessageQueue.Delete(parser.Path);

			log.Information(string.Format(MsmqResources.QueueDropped, Uri));
		}

		public void Purge()
		{
			using (var queue = CreateGuardedQueue())
			{
				queue.Purge();
			}

			log.Information(string.Format(MsmqResources.QueuePurged, Uri));
		}

		public Uri Uri { get; private set; }

		private bool Exists()
		{
			return MessageQueue.Exists(parser.Path);
		}

		public bool IsEmpty()
		{
			try
			{
				using (var queue = CreateGuardedQueue())
				{
					return queue.Peek(timeout) == null;
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
				using (var queue = CreateGuardedQueue())
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

				log.Error(string.Format(MsmqResources.SendMessageIdError, messageId, Uri, ex.CompactMessages()));

				throw;
			}
			catch (Exception ex)
			{
				log.Error(string.Format(MsmqResources.SendMessageIdError, messageId, Uri, ex.CompactMessages()));

				throw;
			}
		}

		public Stream Dequeue()
		{
			try
			{
				Message message;

				using (var queue = CreateGuardedQueue())
				{
					message = queue.Receive(timeout, TransactionType());
				}

				if (message == null)
				{
					return null;
				}

				return message.BodyStream;
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

				log.Error(string.Format(MsmqResources.DequeueError, Uri, ex.Message));

				throw;
			}
			catch (Exception ex)
			{
				log.Error(string.Format(MsmqResources.DequeueError, Uri, ex.CompactMessages()));

				throw;
			}
		}

		public Stream Dequeue(Guid messageId)
		{
			try
			{
				Message message;

				using (var queue = CreateGuardedQueue())
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

				log.Error(string.Format(MsmqResources.DequeueMessageIdError, messageId, Uri, ex.CompactMessages()));

				throw;
			}
			catch (Exception ex)
			{
				log.Error(string.Format(MsmqResources.DequeueMessageIdError, messageId, Uri, ex.CompactMessages()));

				throw;
			}
		}

		public bool Remove(Guid messageId)
		{
			try
			{
				using (var queue = CreateGuardedQueue())
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

				log.Error(string.Format(MsmqResources.RemoveError, messageId, Uri, ex.CompactMessages()));

				throw;
			}
			catch (Exception ex)
			{
				log.Error(string.Format(MsmqResources.RemoveError, messageId, Uri, ex.CompactMessages()));

				throw;
			}
		}

		public IEnumerable<Stream> Read(int top)
		{
			var result = new List<Stream>();

			var count = 0;

			try
			{
				using (var queue = CreateGuardedQueue())
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

				log.Error(string.Format(MsmqResources.ReadError, top, Uri, ex.CompactMessages()));
			}
			catch (Exception ex)
			{
				log.Error(string.Format(MsmqResources.ReadError, top, Uri, ex.CompactMessages()));
			}

			return result;
		}

		private MessageQueue CreateGuardedQueue()
		{
			var messageQueue = new MessageQueue(parser.Path);

			var messagePropertyFilter = new MessagePropertyFilter();

			messagePropertyFilter.SetAll();

			messageQueue.MessageReadPropertyFilter = messagePropertyFilter;

			return messageQueue;
		}

		private void AccessDenied()
		{
			log.Fatal(
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
				return queue.Peek(timeout, cursor, action);
			}
			catch
			{
				return null;
			}
		}

		private MessageQueueTransactionType TransactionType()
		{
			return parser.Transactional
				       ? Transaction.Current != null
					         ? MessageQueueTransactionType.Automatic
					         : parser.Transactional
						           ? MessageQueueTransactionType.Single
						           : MessageQueueTransactionType.None
				       : MessageQueueTransactionType.None;
		}
	}
}