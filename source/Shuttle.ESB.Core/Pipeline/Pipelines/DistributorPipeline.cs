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
				.WithEvent<OnAfterDeserializeTransportMessage>()
				.WithEvent<OnHandleDistributeMessage>()
				.WithEvent<OnAfterHandleDistributeMessage>()
				.WithEvent<OnSerializeTransportMessage>()
				.WithEvent<OnAfterSerializeTransportMessage>()
				.WithEvent<OnDispatchTransportMessage>()
				.WithEvent<OnAfterDispatchTransportMessage>()
				.WithEvent<OnAcknowledgeMessage>()
				.WithEvent<OnAfterAcknowledgeMessage>();

			RegisterObserver(new GetWorkMessageObserver());
			RegisterObserver(new DeserializeTransportMessageObserver());
			RegisterObserver(new DistributorMessageObserver());
			RegisterObserver(new SerializeTransportMessageObserver());
			RegisterObserver(new DispatchTransportMessageObserver());
			RegisterObserver(new AcknowledgeMessageObserver());

			RegisterObserver(new DistributorExceptionObserver()); // must be last
		}

		public override sealed void Obtained()
		{
			base.Obtained();

			State.SetWorkQueue(_bus.Configuration.Inbox.WorkQueue);
			State.SetErrorQueue(_bus.Configuration.Inbox.ErrorQueue);
		}
	}
}