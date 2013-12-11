namespace Shuttle.ESB.Core
{
	public class DeferredMessagePipeline : MessagePipeline
	{
		public DeferredMessagePipeline(IServiceBus bus)
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

			RegisterObserver(new DeferredMessageDequeueObserver());
			RegisterObserver(new DeferredMessageDeserializeTransportMessageObserver());
			RegisterObserver(new SendMessageObserver());
			RegisterObserver(new TransactionScopeObserver());
		}
	}
}