namespace Shuttle.ESB.Core
{
    public abstract class ReceiveMessagePipeline : MessagePipeline
    {
        protected ReceiveMessagePipeline(IServiceBus bus, bool hasJournalQueue)
            : base(bus)
        {
            State.Add(StateKeys.HasJournalQueue, hasJournalQueue);

            if (hasJournalQueue)
            {
                RegisterStage("Read")
                    .WithEvent<OnStartTransactionScope>()
                    .WithEvent<OnDequeue>()
                    .WithEvent<OnDeserializeTransportMessage>()
                    .WithEvent<OnDecompressMessage>()
                    .WithEvent<OnDecryptMessage>()
                    .WithEvent<OnDeserializeMessage>()
                    .WithEvent<OnEnqueueJournal>()
                    .WithEvent<OnCompleteTransactionScope>()
                    .WithEvent<OnDisposeTransactionScope>();

                RegisterStage("Handle")
                    .WithEvent<OnStartTransactionScope>()
                    .WithEvent<OnMessageReceived>()
                    .WithEvent<OnHandleMessage>()
                    .WithEvent<OnMessageHandled>()
                    .WithEvent<OnRemoveJournalMessage>()
                    .WithEvent<OnCompleteTransactionScope>()
                    .WithEvent<OnDisposeTransactionScope>()
                    .WithEvent<OnAcknowledgeMessage>();

                RegisterObserver(new EnqueueJournalObserver());
                RegisterObserver(new RemoveJournalMessageObserver());
            }
            else
            {
                RegisterStage("Read")
                    .WithEvent<OnStartTransactionScope>()
                    .WithEvent<OnDequeue>()
                    .WithEvent<OnDeserializeTransportMessage>()
                    .WithEvent<OnDecompressMessage>()
                    .WithEvent<OnDecryptMessage>()
                    .WithEvent<OnDeserializeMessage>();

                RegisterStage("Handle")
                    .WithEvent<OnMessageReceived>()
                    .WithEvent<OnHandleMessage>()
                    .WithEvent<OnMessageHandled>()
                    .WithEvent<OnCompleteTransactionScope>()
                    .WithEvent<OnDisposeTransactionScope>()
					.WithEvent<OnAcknowledgeMessage>();
			}

            RegisterObserver(new DequeueObserver());
            RegisterObserver(new DeserializeTransportMessageObserver());
            RegisterObserver(new DeserializeMessageObserver());
            RegisterObserver(new DecryptMessageObserver());
            RegisterObserver(new DecompressMessageObserver());
            RegisterObserver(new HandleMessageObserver());
            RegisterObserver(new ReceiveExceptionObserver());
            RegisterObserver(new TransactionScopeObserver());
            RegisterObserver(new AcknowledgeMessageObserver());
			RegisterObserver(new ReceiveMessageStateObserver());
		}
    }
}