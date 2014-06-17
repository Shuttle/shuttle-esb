using System;
using System.Collections.Generic;
using System.Security.Principal;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class TransportMessageConfigurator
	{
		public Guid MessageReceivedId { get; set; }
		public string CorrelationId { get; set; }
		public List<TransportHeader> Headers { get; set; }
		public object Message { get; private set; }
		public DateTime IgnoreTillDate { get; set; }
		public IQueue Queue { get; set; }

		public TransportMessageConfigurator(object message)
		{
			Guard.AgainstNull(message, "message");

			MessageReceivedId = Guid.Empty;
			CorrelationId = string.Empty;
			Headers = new List<TransportHeader>();
			Message = message;
			IgnoreTillDate = DateTime.MinValue;
			Queue = null;
		}

		public TransportMessage CreateTransportMessage(IServiceBusConfiguration configuration)
		{
			var identity = WindowsIdentity.GetCurrent();

			var result = new TransportMessage
				{
					RecipientInboxWorkQueueUri = Queue != null
						                             ? Queue.Uri.ToString()
						                             : string.Empty,
					SenderInboxWorkQueueUri = configuration.HasInbox
						                          ? configuration.Inbox.WorkQueue.Uri.ToString()
						                          : string.Empty,
					PrincipalIdentityName = identity != null
						                        ? identity.Name
						                        : WindowsIdentity.GetAnonymous().Name,
					SendDate = DateTime.Now,
					IgnoreTillDate = IgnoreTillDate,
					MessageType = Message.GetType().FullName,
					AssemblyQualifiedName = Message.GetType().AssemblyQualifiedName,
					EncryptionAlgorithm = configuration.EncryptionAlgorithm,
					CompressionAlgorithm = configuration.CompressionAlgorithm,
					MessageReceivedId = MessageReceivedId,
					CorrelationId = CorrelationId
				};

			result.Headers.Merge(Headers);

			return result;
		}

		public void Merge(TransportMessage transportMessage)
		{
			Guard.AgainstNull(transportMessage, "transportMessage");

			Headers.Merge(transportMessage.Headers);
			CorrelationId = transportMessage.CorrelationId;
		}
	}
}