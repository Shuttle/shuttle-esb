using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class TransportMessageSerializationEventArgs : PipelineEventEventArgs
	{
		public TransportMessageSerializationEventArgs(PipelineEvent pipelineEvent, TransportMessage transportMessage)
			: base(pipelineEvent)
		{
			TransportMessage = transportMessage;
		}

		public TransportMessage TransportMessage { get; private set; }
	}
}