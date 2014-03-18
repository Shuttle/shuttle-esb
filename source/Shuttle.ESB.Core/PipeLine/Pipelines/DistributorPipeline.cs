namespace Shuttle.ESB.Core
{
	public class DistributorPipeline : MessagePipeline
	{
		public DistributorPipeline(IServiceBus bus)
			: base(bus)
		{
			RegisterStage("Distribute")
				.WithEvent<OnStartTransactionScope>()
				.WithEvent<OnDequeue>()
				.WithEvent<OnDeserializeTransportMessage>()
				.WithEvent<OnHandleDistributeMessage>()
				.WithEvent<OnSerializeTransportMessage>()
				.WithEvent<OnSendMessage>()
				.WithEvent<OnCompleteTransactionScope>()
				.WithEvent<OnDisposeTransactionScope>();

			RegisterObserver(new DequeueObserver());
			RegisterObserver(new DeserializeTransportMessageObserver());
			RegisterObserver(new DistributorMessageObserver());
			RegisterObserver(new SerializeTransportMessageObserver());
			RegisterObserver(new SendMessageObserver());
			RegisterObserver(new DistributorExceptionObserver());
			RegisterObserver(new TransactionScopeObserver());
		}

		public override sealed void Obtained()
		{
			base.Obtained();

			SetWorkQueue(_bus.Configuration.Inbox.WorkQueue);
			SetErrorQueue(_bus.Configuration.Inbox.ErrorQueue);
		}
	}
}