using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class QueueMessageEventArgs : PipelineEventEventArgs
	{
		public QueueMessageEventArgs(PipelineEvent pipelineEvent, IQueue queue, TransportMessage message)
			: base(pipelineEvent)
		{
			Queue = queue;
			TransportMessage = message;
		}

		public IQueue Queue { get; private set; }
		public TransportMessage TransportMessage { get; private set; }
	}
}