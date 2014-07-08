namespace Shuttle.ESB.Core
{
	public abstract class ReceiveMessagePipeline : MessagePipeline
	{
		protected ReceiveMessagePipeline(IServiceBus bus)
			: base(bus)
		{
			RegisterStage("Read")
				.WithEvent<OnGetMessage>()
				.WithEvent<OnAfterGetMessage>()
				.WithEvent<OnDeserializeTransportMessage>()
				.WithEvent<OnAfterDeserializeTransportMessage>()
				.WithEvent<OnDecompressMessage>()
				.WithEvent<OnAfterDecompressMessage>()
				.WithEvent<OnDecryptMessage>()
				.WithEvent<OnAfterDecryptMessage>()
				.WithEvent<OnDeserializeMessage>()
				.WithEvent<OnAfterDeserializeMessage>();

			RegisterStage("Handle")
				.WithEvent<OnStartTransactionScope>()
				.WithEvent<OnHandleMessage>()
				.WithEvent<OnAfterHandleMessage>()
				.WithEvent<OnCompleteTransactionScope>()
				.WithEvent<OnDisposeTransactionScope>()
				.WithEvent<OnSendDeferred>()
				.WithEvent<OnAfterSendDeferred>()
				.WithEvent<OnAcknowledgeMessage>()
				.WithEvent<OnAfterAcknowledgeMessage>();

			RegisterObserver(new GetWorkMessageObserver());
			RegisterObserver(new DeserializeTransportMessageObserver());
			RegisterObserver(new DeferTransportMessageObserver());
			RegisterObserver(new DeserializeMessageObserver());
			RegisterObserver(new DecryptMessageObserver());
			RegisterObserver(new DecompressMessageObserver());
			RegisterObserver(new HandleMessageObserver());
			RegisterObserver(new TransactionScopeObserver());
			RegisterObserver(new AcknowledgeMessageObserver());
			RegisterObserver(new SendDeferredObserver());

			RegisterObserver(new ReceiveExceptionObserver()); // must be last
		}
	}
}