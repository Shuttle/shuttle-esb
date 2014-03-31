namespace Shuttle.ESB.Core
{
	public class DistributorPipeline : MessagePipeline
	{
		public DistributorPipeline(IServiceBus bus)
			: base(bus)
		{
			RegisterStage("Distribute")
				.WithEvent<OnGetMessage>()
				.WithEvent<OnDeserializeTransportMessage>()
				.WithEvent<OnHandleDistributeMessage>()
				.WithEvent<OnSerializeTransportMessage>()
				.WithEvent<OnSendMessage>();

			RegisterObserver(new DequeueWorkMessageObserver());
			RegisterObserver(new DeserializeTransportMessageObserver());
			RegisterObserver(new DistributorMessageObserver());
			RegisterObserver(new SerializeTransportMessageObserver());
			RegisterObserver(new SendMessageObserver());
			RegisterObserver(new DistributorExceptionObserver());
		}

		public override sealed void Obtained()
		{
			base.Obtained();

			SetWorkQueue(_bus.Configuration.Inbox.WorkQueue);
			SetErrorQueue(_bus.Configuration.Inbox.ErrorQueue);
		}
	}
}