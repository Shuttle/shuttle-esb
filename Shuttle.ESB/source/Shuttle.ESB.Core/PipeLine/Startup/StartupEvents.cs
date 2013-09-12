using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class OnInitializeQueueFactories : PipelineEvent {}
	public class OnInitializeMessageSerializer : PipelineEvent {}
	public class OnCreateQueues : PipelineEvent {}
	public class OnInitializeMessageHandlerFactory : PipelineEvent {}
	public class OnInitializeMessageRouteProvider : PipelineEvent {}
	public class OnInitializeModules : PipelineEvent {}
	public class OnInitializePipelineFactory : PipelineEvent {}
	public class OnInitializeSubscriptionManager : PipelineEvent {}
	public class OnStartInboxProcessing : PipelineEvent {}
	public class OnStartControlInboxProcessing : PipelineEvent {}
	public class OnStartOutboxProcessing : PipelineEvent {}
	public class OnStartWorker : PipelineEvent {}
	public class OnRecoverInboxJournal : PipelineEvent {}
	public class OnRecoverControlInboxJournal : PipelineEvent { }
	public class OnStarting : PipelineEvent {}
}