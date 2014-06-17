using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class MessageSenderContext
	{
		public MessageSenderContext(TransportMessage transportMessage)
			: this(transportMessage, null)
		{
		}

		public MessageSenderContext(TransportMessage transportMessage, TransportMessage transportMessageReceived)
		{
			Guard.AgainstNull(transportMessage, "transportMessage");

			TransportMessage = transportMessage;
			TransportMessageReceived = transportMessageReceived;
		}

		public TransportMessage TransportMessage { get; private set; }
		public TransportMessage TransportMessageReceived { get; private set; }

		public bool HasTransportMessageReceived
		{
			get { return TransportMessageReceived != null; }
		}
	}
}