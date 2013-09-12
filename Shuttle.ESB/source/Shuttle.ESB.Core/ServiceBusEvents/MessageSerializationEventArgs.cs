using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class MessageSerializationEventArgs : PipelineEventEventArgs
	{
		public MessageSerializationEventArgs(PipelineEvent pipelineEvent, TransportMessage transportMessage, object message)
			: base(pipelineEvent)
		{
			TransportMessage = transportMessage;
			Message = message;
		}

		public TransportMessage TransportMessage { get; private set; }
		public object Message { get; private set; }
	}
}