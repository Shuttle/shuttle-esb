using System;
using System.Collections.Generic;
using System.Security.Principal;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class TransportMessageConfigurator
	{
		private bool _sendToSelf;
		private string _recipientInboxWorkQueueUri;
		private TransportMessage _transportMessage;
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
			_sendToSelf = false;
		}

		public TransportMessage CreateTransportMessage(IServiceBusConfiguration configuration)
		{
			if (_sendToSelf && !configuration.HasInbox)
			{
				throw new InvalidOperationException(ESBResources.SendToSelfException);
			}

			var identity = WindowsIdentity.GetCurrent();

			var result = new TransportMessage
				{
					RecipientInboxWorkQueueUri = _sendToSelf ? configuration.Inbox.WorkQueue.Uri.ToString() : _recipientInboxWorkQueueUri,
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
					MessageReceivedId = HasTransportMessageReceived ? _transportMessage.MessageId : Guid.Empty,
					CorrelationId = _correlationId
				};

			result.Headers.Merge(Headers);

			return result;
		}

		public bool HasTransportMessageReceived
		{
			get { return _transportMessage != null; }
		}

		public void TransportMessageReceived(TransportMessage transportMessage)
		{
			Guard.AgainstNull(transportMessage, "transportMessage");

			_transportMessage = transportMessage;

			Headers.Merge(transportMessage.Headers);
			_correlationId = transportMessage.CorrelationId;
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

		public TransportMessageConfigurator SendToRecipient(IQueue queue)
		{
			return SendToRecipient(queue.Uri.ToString());
		}

		public TransportMessageConfigurator SendToRecipient(Uri uri)
		{
			return SendToRecipient(uri.ToString());
		}

		public TransportMessageConfigurator SendToRecipient(string uri)
		{
			_sendToSelf = false;

			_recipientInboxWorkQueueUri = uri;

			return this;
		}

		public TransportMessageConfigurator SendToSelf()
		{
			_sendToSelf = true;

			return this;
		}
	}
}