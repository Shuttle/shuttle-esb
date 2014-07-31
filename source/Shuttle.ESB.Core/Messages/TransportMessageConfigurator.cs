using System;
using System.Collections.Generic;
using System.Security.Principal;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class TransportMessageConfigurator
	{
		private bool _local;
		private string _recipientInboxWorkQueueUri;
		private TransportMessage _transportMessageReceived;
		private DateTime _ignoreTillDate;
		private string _correlationId;

		public List<TransportHeader> Headers { get; set; }
		public object Message { get; private set; }

		public TransportMessageConfigurator(object message)
		{
			Guard.AgainstNull(message, "message");

			Headers = new List<TransportHeader>();
			Message = message;

			_correlationId = string.Empty;
			_ignoreTillDate = DateTime.MinValue;
			_recipientInboxWorkQueueUri = null;
			_local = false;
		}

		public TransportMessage TransportMessage(IServiceBusConfiguration configuration)
		{
			if (_local && !configuration.HasInbox)
			{
				throw new InvalidOperationException(ESBResources.SendToSelfException);
			}

			var identity = WindowsIdentity.GetCurrent();

			var result = new TransportMessage
				{
					RecipientInboxWorkQueueUri = _local ? configuration.Inbox.WorkQueue.Uri.ToString() : _recipientInboxWorkQueueUri,
					SenderInboxWorkQueueUri = configuration.HasInbox
						                          ? configuration.Inbox.WorkQueue.Uri.ToString()
						                          : string.Empty,
					PrincipalIdentityName = identity != null
						                        ? identity.Name
						                        : WindowsIdentity.GetAnonymous().Name,
					IgnoreTillDate = _ignoreTillDate,
					MessageType = Message.GetType().FullName,
					AssemblyQualifiedName = Message.GetType().AssemblyQualifiedName,
					EncryptionAlgorithm = configuration.EncryptionAlgorithm,
					CompressionAlgorithm = configuration.CompressionAlgorithm,
					MessageReceivedId = HasTransportMessageReceived ? _transportMessageReceived.MessageId : Guid.Empty,
					CorrelationId = _correlationId,
					SendDate = DateTime.Now
				};

			result.Headers.Merge(Headers);

			return result;
		}

		public bool HasTransportMessageReceived
		{
			get { return _transportMessageReceived != null; }
		}

		public void TransportMessageReceived(TransportMessage transportMessageReceived)
		{
			Guard.AgainstNull(transportMessageReceived, "transportMessageReceived");

			_transportMessageReceived = transportMessageReceived;

			Headers.Merge(transportMessageReceived.Headers);
			_correlationId = transportMessageReceived.CorrelationId;
		}

		public TransportMessageConfigurator Defer(DateTime ignoreTillDate)
		{
			_ignoreTillDate = ignoreTillDate;

			return this;
		}

		public TransportMessageConfigurator WithCorrelationId(string correlationId)
		{
			_correlationId = correlationId;

			return this;
		}

		public TransportMessageConfigurator WithRecipient(IQueue queue)
		{
			return WithRecipient(queue.Uri.ToString());
		}

		public TransportMessageConfigurator WithRecipient(Uri uri)
		{
			return WithRecipient(uri.ToString());
		}

		public TransportMessageConfigurator WithRecipient(string uri)
		{
			_local = false;

			_recipientInboxWorkQueueUri = uri;

			return this;
		}

		public TransportMessageConfigurator Local()
		{
			_local = true;

			return this;
		}

		public TransportMessageConfigurator Reply()
		{
			if (!HasTransportMessageReceived || string.IsNullOrEmpty(_transportMessageReceived.SenderInboxWorkQueueUri))
			{
				throw new InvalidOperationException(ESBResources.SendReplyException);
			}

			_local = false;

			WithRecipient(_transportMessageReceived.SenderInboxWorkQueueUri);

			return this;
		}
	}
}