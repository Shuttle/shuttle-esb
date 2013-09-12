namespace Shuttle.ESB.Core
{
	public abstract class ReceiveMessagePipeline : MessagePipeline
	{
		protected ReceiveMessagePipeline(IServiceBus bus)
			: base(bus)
		{
			OnExecuteRaiseEvent<OnStartTransactionScope>()
				.ThenEvent<OnDequeue>()
				.ThenEvent<OnDeserializeTransportMessage>()
				.ThenEvent<OnDecryptMessage>()
				.ThenEvent<OnDecompressMessage>()
				.ThenEvent<OnDeserializeMessage>()				
				.ThenEvent<OnEnqueueJournal>()
				.ThenEvent<OnCompleteTransactionScope>()
				.ThenEvent<OnDisposeTransactionScope>()
				.ThenEvent<OnStartTransactionScope>()
				.ThenEvent<OnMessageReceived>()
				.ThenEvent<OnHandleMessage>()
				.ThenEvent<OnCompleteTransactionScope>()
				.ThenEvent<OnDisposeTransactionScope>()
				.ThenEvent<OnRemoveJournalMessage>();

			RegisterObserver(new DequeueObserver())
				.AndObserver(new DeserializeTransportMessageObserver())
				.AndObserver(new DecryptMessageObserver())
				.AndObserver(new DeserializeMessageObserver())
				.AndObserver(new DecompressMessageObserver())
				.AndObserver(new EnqueueJournalObserver())
				.AndObserver(new HandleMessageObserver())
				.AndObserver(new RemoveJournalMessageObserver())
				.AndObserver(new ExceptionObserver())
				.AndObserver(new TransactionScopeObserver());
		}
	}
}