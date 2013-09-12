namespace Shuttle.ESB.Core
{
	public class OutboxPipeline : MessagePipeline
	{
		public OutboxPipeline(IServiceBus bus)
			: base(bus)
		{
			Obtained();

			// read outbox		
			OnExecuteRaiseEvent<OnStartTransactionScope>()
				.ThenEvent<OnDequeue>()
				.ThenEvent<OnDeserializeTransportMessage>()
				.ThenEvent<OnDecryptMessage>()
				.ThenEvent<OnDecompressMessage>()
				.ThenEvent<OnDeserializeMessage>()
				.ThenEvent<OnMessageReceived>()

				// send message
				.ThenEvent<OnPrepareMessage>()
				.ThenEvent<OnSerializeMessage>()
				.ThenEvent<OnCompressMessage>()
				.ThenEvent<OnEncryptMessage>()
				.ThenEvent<OnSerializeTransportMessage>()
				.ThenEvent<OnSendMessage>()
				.ThenEvent<OnAfterSendMessage>()
				.ThenEvent<OnCompleteTransactionScope>()
				.ThenEvent<OnDisposeTransactionScope>();

			RegisterObserver(new TransactionScopeObserver())

				// read observers
				.AndObserver(new DequeueObserver())
				.AndObserver(new DecryptMessageObserver())
				.AndObserver(new DeserializeMessageObserver())
				.AndObserver(new DeserializeTransportMessageObserver())
				.AndObserver(new DecompressMessageObserver())

				// send observers
				.AndObserver(new PrepareMessageObserver())
				.AndObserver(new SerializeMessageObserver())
				.AndObserver(new SerializeTransportMessageObserver())
				.AndObserver(new EncryptMessageObserver())
				.AndObserver(new CompressMessageObserver())
				.AndObserver(new SendOutboxMessageObserver())
				
				.AndObserver(new ExceptionObserver());
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