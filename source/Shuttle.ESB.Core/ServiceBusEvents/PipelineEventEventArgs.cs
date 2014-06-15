namespace Shuttle.ESB.Core
{
	public class PipelineEventEventArgs
	{
		public PipelineEventEventArgs(PipelineEvent pipelineEvent)
		{
			PipelineEvent = pipelineEvent;
		}

		public PipelineEvent PipelineEvent { get; private set; }
	}
}