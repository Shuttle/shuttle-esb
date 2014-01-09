using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Messaging;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Transactions;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Msmq
{
	public class MsmqQueue : IQueue, ICreate, IDrop, IPurge, ICount, IQueueReader
	{
		internal const string SCHEME = "msmq";

		private readonly TimeSpan timeout;

		[ThreadStatic] private static object underlyingMessageData;

		private readonly string path;
		private readonly string host;
		private readonly bool usesIPAddress;

		private readonly Regex regexIPAddress =
			new Regex(
				@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");

		private readonly ILog log;

		public MsmqQueue(Uri uri, MsmqConfiguration msmqConfiguration)
		{
			Guard.AgainstNull(uri, "uri");

			if (!uri.Scheme.Equals(SCHEME, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new InvalidSchemeException(SCHEME, uri.ToString());
			}

			log = Log.For(this);

			var builder = new UriBuilder(uri);

			host = uri.Host;

			if (host.Equals("."))
			{
				builder.Host = Environment.MachineName.ToLower();
			}

			if (uri.LocalPath == "/")
			{
				throw new UriFormatException(string.Format(ESBResources.UriFormatException, "msmq://{{host-name}}/{{queue-name}}",
				                                           uri));
			}

			Uri = builder.Uri;

			IsLocal = Uri.Host.Equals(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase);

			var queueConfiguration = msmqConfiguration.FindQueueConfiguration(uri);

			IsTransactional = queueConfiguration != null && queueConfiguration.IsTransactional;

			usesIPAddress = regexIPAddress.IsMatch(host);

			path = IsLocal
				       ? string.Format(@"{0}\private$\{1}", host, uri.Segments[1])
				       : usesIPAddress
					         ? string.Format(@"FormatName:DIRECT=TCP:{0}\private$\{1}", host, uri.Segments[1])
					         : string.Format(@"FormatName:DIRECT=OS:{0}\private$\{1}", host, uri.Segments[1]);

			timeout = IsLocal
				          ? TimeSpan.FromMilliseconds(msmqConfiguration.LocalQueueTimeoutMilliseconds )
				          : TimeSpan.FromMilliseconds(msmqConfiguration.RemoteQueueTimeoutMilliseconds);
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
			if (Exists() != QueueAvailability.Missing)
			{
				return;
			}

			if (!IsLocal)
			{
				throw new InvalidOperationException(string.Format(MsmqResources.CannotCreateRemoteQueue, Uri));
			}

			MessageQueue.Create(path, IsTransactional).Dispose();

			log.Information(string.Format(MsmqResources.QueueCreated, Uri));
		}

		public void Drop()
		{
			if (Exists() == QueueAvailability.Missing)
			{
				return;
			}

			if (!IsLocal)
			{
				throw new InvalidOperationException(string.Format(MsmqResources.CannotDropRemoteQueue, Uri));
			}

			MessageQueue.Delete(path);

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

		public bool IsTransactional { get; private set; }

		public bool IsLocal { get; private set; }
		public Uri Uri { get; private set; }

		public QueueAvailability Exists()
		{
			return IsLocal
				       ? MessageQueue.Exists(path)
					         ? QueueAvailability.Exists
					         : QueueAvailability.Missing
				       : QueueAvailability.Unknown;
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
			ResetUnderlyingMessageData();

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

				underlyingMessageData = message;

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
			ResetUnderlyingMessageData();

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

				underlyingMessageData = message;

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
			var messageQueue = new MessageQueue(path);

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
					path));

			if (Environment.UserInteractive)
			{
				return;
			}

			Process.GetCurrentProcess().Kill();
		}

		private void ResetUnderlyingMessageData()
		{
			underlyingMessageData = null;
		}

		private Message PeekMessage(MessageQueue queue, Cursor cursor, PeekAction action)
		{
			ResetUnderlyingMessageData();

			try
			{
				underlyingMessageData = queue.Peek(timeout, cursor, action);

				return (Message) underlyingMessageData;
			}
			catch
			{
				return null;
			}
		}

		private MessageQueueTransactionType TransactionType()
		{
			return IsTransactional
				       ? Transaction.Current != null
					         ? MessageQueueTransactionType.Automatic
					         : IsTransactional
						           ? MessageQueueTransactionType.Single
						           : MessageQueueTransactionType.None
				       : MessageQueueTransactionType.None;
		}
	}
}