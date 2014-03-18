namespace Shuttle.ESB.Core
{
	public class OutboxPipeline : MessagePipeline
	{
		public OutboxPipeline(IServiceBus bus)
			: base(bus)
		{
			RegisterStage("Read")
				.WithEvent<OnDequeue>()
				.WithEvent<OnDeserializeTransportMessage>();

			RegisterStage("Send")
				.WithEvent<OnSendMessage>()
				.WithEvent<OnAcknowledgeMessage>();

			RegisterObserver(new DequeueObserver());
			RegisterObserver(new DeserializeTransportMessageObserver());
			RegisterObserver(new SendOutboxMessageObserver());

			RegisterObserver(new AcknowledgeMessageObserver());
			RegisterObserver(new OutboxExceptionObserver());
		}

		public override sealed void Obtained()
		{
			base.Obtained();

			SetWorkQueue(_bus.Configuration.Outbox.WorkQueue);
			SetErrorQueue(_bus.Configuration.Outbox.ErrorQueue);

			SetDurationToIgnoreOnFailure(_bus.Configuration.Outbox.DurationToIgnoreOnFailure);
			SetMaximumFailureCount(_bus.Configuration.Outbox.MaximumFailureCount);
		}
	}
}