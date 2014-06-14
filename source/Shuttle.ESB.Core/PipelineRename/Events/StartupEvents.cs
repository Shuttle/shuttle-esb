using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class OnRegisterSharedConfiguration : PipelineEvent { }
	public class OnAfterRegisterSharedConfiguration : PipelineEvent { }
	public class OnRegisterControlInboxQueueConfiguration : PipelineEvent { }
	public class OnAfterRegisterControlInboxQueueConfiguration : PipelineEvent { }
	public class OnRegisterInboxQueueConfiguration : PipelineEvent { }
	public class OnAfterRegisterInboxQueueConfiguration : PipelineEvent { }
	public class OnRegisterOutboxQueueConfiguration : PipelineEvent { }
	public class OnAfterRegisterOutboxQueueConfiguration : PipelineEvent { }
	public class OnRegisterWorkerConfiguration : PipelineEvent { }
	public class OnAfterRegisterWorkerConfiguration : PipelineEvent { }
	
	public class OnInitializeQueueFactories : PipelineEvent {}
	public class OnAfterInitializeQueueFactories : PipelineEvent { }
	public class OnCreateQueues : PipelineEvent {}
	public class OnAfterCreateQueues : PipelineEvent { }
	public class OnInitializeMessageHandlerFactory : PipelineEvent {}
	public class OnAfterInitializeMessageHandlerFactory : PipelineEvent { }
	public class OnInitializeMessageRouteProvider : PipelineEvent {}
	public class OnAfterInitializeMessageRouteProvider : PipelineEvent { }
	public class OnInitializeForwardingRouteProvider : PipelineEvent {}
	public class OnAfterInitializeForwardingRouteProvider : PipelineEvent { }
	public class OnInitializePipelineFactory : PipelineEvent {}
	public class OnAfterInitializePipelineFactory : PipelineEvent { }
	public class OnInitializeQueueManager : PipelineEvent {}
	public class OnAfterInitializeQueueManager : PipelineEvent { }
	public class OnInitializeSubscriptionManager : PipelineEvent {}
	public class OnAfterInitializeSubscriptionManager : PipelineEvent { }
	public class OnInitializeIdempotenceService : PipelineEvent {}
	public class OnAfterInitializeIdempotenceService : PipelineEvent { }
	public class OnInitializeTransactionScopeFactory : PipelineEvent {}
	public class OnAfterInitializeTransactionScopeFactory : PipelineEvent { }
	public class OnStartInboxProcessing : PipelineEvent {}
	public class OnAfterStartInboxProcessing : PipelineEvent { }
	public class OnStartControlInboxProcessing : PipelineEvent {}
	public class OnAfterStartControlInboxProcessing : PipelineEvent { }
	public class OnStartOutboxProcessing : PipelineEvent {}
	public class OnAfterStartOutboxProcessing : PipelineEvent { }
	public class OnStartDeferredMessageProcessing : PipelineEvent {}
	public class OnAfterStartDeferredMessageProcessing : PipelineEvent { }
	public class OnStartWorker : PipelineEvent {}
	public class OnAfterStartWorker : PipelineEvent { }
	public class OnStarting : PipelineEvent {}
}