namespace Shuttle.ESB.Core
{
	public class OutboxPipeline : MessagePipeline
	{
		public OutboxPipeline(IServiceBus bus)
			: base(bus)
		{
			RegisterStage("Read")
				.WithEvent<OnGetMessage>()
				.WithEvent<OnAfterGetMessage>()
				.WithEvent<OnDeserializeTransportMessage>()
				.WithEvent<OnAfterDeserializeTransportMessage>();

			RegisterStage("Send")
				.WithEvent<OnDispatchTransportMessage>()
				.WithEvent<OnAfterDispatchTransportMessage>()
				.WithEvent<OnAcknowledgeMessage>()
				.WithEvent<OnAfterAcknowledgeMessage>();

			RegisterObserver(new GetWorkMessageObserver());
			RegisterObserver(new DeserializeTransportMessageObserver());
			RegisterObserver(new SendOutboxMessageObserver());

			RegisterObserver(new AcknowledgeMessageObserver());
			RegisterObserver(new OutboxExceptionObserver());
		}

		public override sealed void Obtained()
		{
			base.Obtained();

			State.SetWorkQueue(_bus.Configuration.Outbox.WorkQueue);
			State.SetErrorQueue(_bus.Configuration.Outbox.ErrorQueue);

			State.SetDurationToIgnoreOnFailure(_bus.Configuration.Outbox.DurationToIgnoreOnFailure);
			State.SetMaximumFailureCount(_bus.Configuration.Outbox.MaximumFailureCount);
		}
	}
}