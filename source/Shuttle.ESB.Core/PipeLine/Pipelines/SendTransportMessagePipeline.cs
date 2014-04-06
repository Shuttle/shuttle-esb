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
	}
}