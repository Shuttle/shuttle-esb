using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class StartupPipeline : ObservablePipeline
	{
		public StartupPipeline(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			State.Add(bus);

			RegisterStage("Initializing")
				.WithEvent<OnInitializeQueueFactories>()
				.WithEvent<OnCreateQueues>()
				.WithEvent<OnInitializeMessageHandlerFactory>()
				.WithEvent<OnInitializeMessageRouteProvider>()
				.WithEvent<OnInitializeForwardingRouteProvider>()
				.WithEvent<OnInitializePipelineFactory>()
				.WithEvent<OnInitializeSubscriptionManager>()
				.WithEvent<OnInitializeIdempotenceService>()
				.WithEvent<OnInitializeTransactionScopeFactory>();

			RegisterStage("Start")
				.WithEvent<OnStartInboxProcessing>()
				.WithEvent<OnStartControlInboxProcessing>()
				.WithEvent<OnStartOutboxProcessing>()
				.WithEvent<OnStartDeferredMessageProcessing>()
				.WithEvent<OnStartWorker>();

			RegisterStage("Final")
				.WithEvent<OnStarting>();

			RegisterObserver(new ServiceBusStartupObserver(bus));
			RegisterObserver(new TransactionScopeObserver());
		}
	}
}