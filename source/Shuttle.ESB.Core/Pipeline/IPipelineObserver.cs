namespace Shuttle.ESB.Core
{
	public interface IObserver
	{
	} 

	public interface IPipelineObserver<TPipelineEvent> : IObserver	
	{
		void Execute(TPipelineEvent  pipelineEvent1);
	}
}