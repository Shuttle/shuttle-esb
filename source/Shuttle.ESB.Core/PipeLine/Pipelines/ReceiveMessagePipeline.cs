namespace Shuttle.ESB.Core
{
	public abstract class ReceiveMessagePipeline : MessagePipeline
	{
		protected ReceiveMessagePipeline(IServiceBus bus)
			: base(bus)
		{
			RegisterStage("Read")
				.WithEvent<OnGetMessage>()
				.WithEvent<OnDeserializeTransportMessage>()
				.WithEvent<OnAfterDeserializeTransportMessage>()
				.WithEvent<OnDecompressMessage>()
				.WithEvent<OnDecryptMessage>()
				.WithEvent<OnDeserializeMessage>();

			RegisterStage("Handle")
				.WithEvent<OnStartTransactionScope>()
				.WithEvent<OnHandleMessage>()
				.WithEvent<OnCompleteTransactionScope>()
				.WithEvent<OnDisposeTransactionScope>()
				.WithEvent<OnSendDeferred>()
				.WithEvent<OnAcknowledgeMessage>();

			RegisterObserver(new DequeueWorkMessageObserver());
			RegisterObserver(new DeserializeTransportMessageObserver());
			RegisterObserver(new DeferTransportMessageObserver());
			RegisterObserver(new DeserializeMessageObserver());
			RegisterObserver(new DecryptMessageObserver());
			RegisterObserver(new DecompressMessageObserver());
			RegisterObserver(new HandleMessageObserver());
			RegisterObserver(new ReceiveExceptionObserver());
			RegisterObserver(new TransactionScopeObserver());
			RegisterObserver(new AcknowledgeMessageObserver());
			RegisterObserver(new SendDeferredObserver());
		}
	}
}