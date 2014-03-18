using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class OnRegisterSharedConfiguration : PipelineEvent { }
	public class OnRegisterControlInboxQueueConfiguration : PipelineEvent { }
	public class OnRegisterInboxQueueConfiguration : PipelineEvent { }
	public class OnRegisterOutboxQueueConfiguration : PipelineEvent { }
	public class OnRegisterWorkerConfiguration : PipelineEvent { }
	
	public class OnInitializeQueueFactories : PipelineEvent {}
	public class OnCreateQueues : PipelineEvent {}
	public class OnInitializeMessageHandlerFactory : PipelineEvent {}
	public class OnInitializeMessageRouteProvider : PipelineEvent {}
	public class OnInitializeForwardingRouteProvider : PipelineEvent {}
	public class OnInitializePipelineFactory : PipelineEvent {}
	public class OnInitializeQueueManager : PipelineEvent {}
	public class OnInitializeSubscriptionManager : PipelineEvent {}
	public class OnInitializeIdempotenceService : PipelineEvent {}
	public class OnInitializeTransactionScopeFactory : PipelineEvent {}
	public class OnStartInboxProcessing : PipelineEvent {}
	public class OnStartControlInboxProcessing : PipelineEvent {}
	public class OnStartOutboxProcessing : PipelineEvent {}
	public class OnStartDeferredMessageProcessing : PipelineEvent {}
	public class OnStartWorker : PipelineEvent {}
	public class OnStarting : PipelineEvent {}
}