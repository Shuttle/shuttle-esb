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

			RegisterObserver(new GetWorkMessageObserver());
			RegisterObserver(new DeserializeTransportMessageObserver());
			RegisterObserver(new DistributorMessageObserver());
			RegisterObserver(new SerializeTransportMessageObserver());
			RegisterObserver(new SendMessageObserver());
			RegisterObserver(new DistributorExceptionObserver());
		}

		public override sealed void Obtained()
		{
			base.Obtained();

			State.SetWorkQueue(_bus.Configuration.Inbox.WorkQueue);
			State.SetErrorQueue(_bus.Configuration.Inbox.ErrorQueue);
		}
	}
}