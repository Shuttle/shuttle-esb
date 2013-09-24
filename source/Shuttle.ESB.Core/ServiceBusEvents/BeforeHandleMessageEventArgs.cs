using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class BeforeHandleMessageEventArgs : PipelineEventEventArgs
	{
		public BeforeHandleMessageEventArgs(PipelineEvent pipelineEvent, TransportMessage transportMessage)
			: base(pipelineEvent)
		{
			TransportMessage = transportMessage;
		}

		public TransportMessage TransportMessage { get; private set; }
	}
}