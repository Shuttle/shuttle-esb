namespace Shuttle.ESB.Core
{
	public interface ITransactionScopeFactory
	{
		ITransactionScope Create(PipelineEvent pipelineEvent);
	}
}