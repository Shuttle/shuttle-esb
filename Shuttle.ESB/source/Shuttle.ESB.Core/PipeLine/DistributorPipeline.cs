namespace Shuttle.ESB.Core
{
	public class DistributorPipeline : MessagePipeline
	{
		public DistributorPipeline(IServiceBus bus)
			: base(bus)
		{
			Obtained();

			OnExecuteRaiseEvent<OnGetAvilableWorker>()
				.ThenEvent<OnStartTransactionScope>()
				.ThenEvent<OnDequeue>()
				.ThenEvent<OnDeserializeTransportMessage>()
				.ThenEvent<OnDecryptMessage>()
				.ThenEvent<OnDecompressMessage>()
				.ThenEvent<OnDeserializeMessage>()
				.ThenEvent<OnHandleDistributeMessage>()
				.ThenEvent<OnSerializeMessage>()
				.ThenEvent<OnCompressMessage>()
				.ThenEvent<OnEncryptMessage>()
				.ThenEvent<OnSendMessage>()
				.ThenEvent<OnAfterSendMessage>()
				.ThenEvent<OnCompleteTransactionScope>()
				.ThenEvent<OnDisposeTransactionScope>();

			RegisterObserver(new TransactionScopeObserver())
				.AndObserver(new DequeueObserver())
				.AndObserver(new DeserializeTransportMessageObserver())
				.AndObserver(new DecryptMessageObserver())
				.AndObserver(new DecompressMessageObserver())
				.AndObserver(new DeserializeMessageObserver())
				.AndObserver(new DistributorMessageObserver())
				.AndObserver(new SerializeMessageObserver())
				.AndObserver(new CompressMessageObserver())
				.AndObserver(new EncryptMessageObserver())
				.AndObserver(new SendMessageObserver())
				.AndObserver(new ExceptionObserver());
		}

		public override sealed void Obtained()
		{
			base.Obtained();

			WorkQueue = bus.Configuration.Inbox.WorkQueue;
			ErrorQueue = bus.Configuration.Inbox.ErrorQueue;
		}
	}
}