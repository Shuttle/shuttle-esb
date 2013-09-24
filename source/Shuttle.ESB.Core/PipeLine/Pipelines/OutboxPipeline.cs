namespace Shuttle.ESB.Core
{
	public class OutboxPipeline : MessagePipeline
	{
		public OutboxPipeline(IServiceBus bus)
			: base(bus)
		{
			RegisterStage("Read")
				.WithEvent<OnStartTransactionScope>()
				.WithEvent<OnDequeue>()
				.WithEvent<OnDeserializeTransportMessage>();

			RegisterStage("Send")
				.WithEvent<OnSendMessage>()
				.WithEvent<OnCompleteTransactionScope>()
				.WithEvent<OnDisposeTransactionScope>();

			// read observers
			RegisterObserver(new DequeueObserver());
			RegisterObserver(new DeserializeTransportMessageObserver());

			// send observers
			RegisterObserver(new SendOutboxMessageObserver());

			RegisterObserver(new OutboxExceptionObserver());
			RegisterObserver(new TransactionScopeObserver());
		}

		public override sealed void Obtained()
		{
			base.Obtained();

			WorkQueue = bus.Configuration.Outbox.WorkQueue;
			ErrorQueue = bus.Configuration.Outbox.ErrorQueue;

			DurationToIgnoreOnFailure = bus.Configuration.Outbox.DurationToIgnoreOnFailure;
			MaximumFailureCount = bus.Configuration.Outbox.MaximumFailureCount;
		}
	}
}