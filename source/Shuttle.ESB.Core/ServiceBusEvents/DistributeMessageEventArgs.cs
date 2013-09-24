using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DistributeMessageEventArgs : PipelineEventEventArgs
	{
		public IQueue DestinationQueue { get; private set; }
		public TransportMessage TransportMessage { get; private set; }

		public DistributeMessageEventArgs(PipelineEvent pipelineEvent, IQueue destinationQueue,
		                                  TransportMessage transportMessage)
			: base(pipelineEvent)
		{
			DestinationQueue = destinationQueue;
			TransportMessage = transportMessage;
		}
	}
}