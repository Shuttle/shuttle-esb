using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class StartupPipeline : ObservablePipeline
	{
		public StartupPipeline(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			State.Add(bus);

			OnExecuteRaiseEvent<OnInitializeQueueFactories>()
				.ThenEvent<OnInitializeMessageSerializer>()
				.ThenEvent<OnCreateQueues>()
				.ThenEvent<OnInitializeMessageHandlerFactory>()
				.ThenEvent<OnInitializeMessageRouteProvider>()
				.ThenEvent<OnInitializeModules>()
				.ThenEvent<OnInitializePipelineFactory>()
				.ThenEvent<OnInitializeSubscriptionManager>()
				.ThenEvent<OnStartInboxProcessing>()
				.ThenEvent<OnStartControlInboxProcessing>()
				.ThenEvent<OnStartOutboxProcessing>()
				.ThenEvent<OnStartWorker>()

				// recover journal message for inbox
				.ThenEvent<OnStartTransactionScope>()
				.ThenEvent<OnRecoverInboxJournal>()
				.ThenEvent<OnCompleteTransactionScope>()
				.ThenEvent<OnDisposeTransactionScope>()

				// recover journal message for control inbox
				.ThenEvent<OnStartTransactionScope>()
				.ThenEvent<OnRecoverControlInboxJournal>()
				.ThenEvent<OnCompleteTransactionScope>()
				.ThenEvent<OnDisposeTransactionScope>()

				.ThenEvent<OnStarting>();

			RegisterObserver(new ServiceBusStartupObserver(bus))
				.AndObserver(new TransactionScopeObserver());
		}
	}
}