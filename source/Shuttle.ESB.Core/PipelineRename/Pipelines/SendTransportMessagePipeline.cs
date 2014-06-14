namespace Shuttle.ESB.Core
{
	public class SendTransportMessagePipeline : MessagePipeline
	{
		public SendTransportMessagePipeline(IServiceBus bus)
			: base(bus)
		{
			RegisterStage("Send")
				.WithEvent<OnSerializeTransportMessage>()
				.WithEvent<OnAfterSerializeTransportMessage>()
				.WithEvent<OnSendMessage>()
				.WithEvent<OnAfterSendMessage>();

			RegisterObserver(new SerializeTransportMessageObserver());
			RegisterObserver(new SendMessageObserver());
		}
	}
}