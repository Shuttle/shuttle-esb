using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class AfterHandleMessageEventArgs : PipelineEventEventArgs
	{
		public AfterHandleMessageEventArgs(PipelineEvent pipelineEvent, IQueue workQueue, TransportMessage transportMessage)
			: base(pipelineEvent)
		{
			WorkQueue = workQueue;
			TransportMessage = transportMessage;
		}

		public TransportMessage TransportMessage { get; private set; }
		public IQueue WorkQueue { get; private set; }
	}
}