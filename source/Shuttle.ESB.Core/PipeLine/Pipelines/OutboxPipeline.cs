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

			WorkQueue = _bus.Configuration.Outbox.WorkQueue;
			ErrorQueue = _bus.Configuration.Outbox.ErrorQueue;

			DurationToIgnoreOnFailure = _bus.Configuration.Outbox.DurationToIgnoreOnFailure;
			MaximumFailureCount = _bus.Configuration.Outbox.MaximumFailureCount;
		}
	}
}