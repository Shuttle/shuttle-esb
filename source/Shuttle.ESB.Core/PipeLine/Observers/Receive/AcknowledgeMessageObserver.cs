using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class AcknowledgeMessageObserver :
		IPipelineObserver<OnAcknowledgeMessage>
	{
		private readonly ILog _log;

		public AcknowledgeMessageObserver()
		{
			_log = Log.For(this);
		}

		public void Execute(OnAcknowledgeMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;

			if (pipelineEvent.Pipeline.Exception != null && !state.GetTransactionComplete())
			{
				return;
			}

			var transportMessage = state.GetTransportMessage();

			state.GetWorkQueue().Acknowledge(state.GetReceivedMessage().AcknowledgementToken);

			_log.Trace(string.Format(ESBResources.TraceAcknowledge, transportMessage.MessageType, transportMessage.MessageId));
		}
	}
}
