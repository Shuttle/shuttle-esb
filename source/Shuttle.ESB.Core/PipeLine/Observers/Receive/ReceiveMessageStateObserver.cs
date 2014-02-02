using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class ReceiveMessageStateObserver :
		IPipelineObserver<OnHandleMessage>,
        IPipelineObserver<OnAcknowledgeMessage>
    {
        private readonly ILog _log;

        public ReceiveMessageStateObserver()
        {
            _log = Log.For(this);
        }

		public void Execute(OnHandleMessage pipelineEvent)
		{
			var transportMessage = pipelineEvent.GetTransportMessage();

			if (!pipelineEvent.GetServiceBus().Configuration.HasReceiveMessageStateService
				||
				transportMessage == null)
			{
				return;
			}

			pipelineEvent.GetServiceBus().Configuration.ReceiveMessageStateService.HandleMessage(transportMessage);
		}

		public void Execute(OnAcknowledgeMessage pipelineEvent)
        {
            var transportMessage = pipelineEvent.GetTransportMessage();

            if (!pipelineEvent.GetServiceBus().Configuration.HasReceiveMessageStateService
                ||
                transportMessage == null)
            {
                return;
            }

            pipelineEvent.GetServiceBus().Configuration.ReceiveMessageStateService.AcknowledgeMessage(transportMessage);

            if (_log.IsTraceEnabled)
            {
                _log.Trace(string.Format(ESBResources.TraceMessageAcknowledged, transportMessage.MessageType, transportMessage.MessageId));
            }
        }
    }
}
