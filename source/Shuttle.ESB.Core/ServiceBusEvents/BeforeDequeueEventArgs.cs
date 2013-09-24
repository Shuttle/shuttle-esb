using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class BeforeDequeueEventArgs : PipelineEventEventArgs
	{
		public BeforeDequeueEventArgs(PipelineEvent pipelineEvent, IQueue queue)
			: base(pipelineEvent)
		{
			Queue = queue;
		}

		public IQueue Queue { get; private set; }
	}
}