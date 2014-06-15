namespace Shuttle.ESB.Core
{
	public interface IPipeline
	{
	    State<IPipeline> State { get; }
		IExecuteEventThen OnExecuteRaiseEvent(PipelineEvent pipelineEvent);
		IExecuteEventThen OnExecuteRaiseEvent<TPipelineEvent>() where TPipelineEvent : PipelineEvent, new();
		IRegisterObserverAnd RegisterObserver(IObserver observer);
		RegisterEventBefore BeforeEvent<TPipelineEvent>() where TPipelineEvent : PipelineEvent, new();
		RegisterEventAfter AfterEvent<TPipelineEvent>() where TPipelineEvent : PipelineEvent, new();
	}
}