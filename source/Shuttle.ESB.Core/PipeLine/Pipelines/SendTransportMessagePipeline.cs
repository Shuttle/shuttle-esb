using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class SendTransportMessagePipeline : MessagePipeline
	{
		public SendTransportMessagePipeline(IServiceBus bus)
			: base(bus)
		{
			RegisterStage("Send")
				.WithEvent<OnSerializeTransportMessage>()
				.WithEvent<OnSendMessage>();

			RegisterObserver(new SerializeTransportMessageObserver());
			RegisterObserver(new SendMessageObserver());
		}

		public bool Execute(TransportMessage transportMessage)
		{
			Guard.AgainstNull(transportMessage, "transportMessage");

			TransportMessage = transportMessage;

			return base.Execute();
		}
	}
}