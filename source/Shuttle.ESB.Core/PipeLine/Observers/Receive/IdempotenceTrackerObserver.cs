using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class IdempotenceTrackerObserver :
        IPipelineObserver<OnMessageHandled>
    {
        private readonly ILog log;

        public IdempotenceTrackerObserver()
        {
            log = Log.For(this);
        }

        public void Execute(OnMessageHandled pipelineEvent)
        {
            var transportMessage = pipelineEvent.GetTransportMessage();

            if (!pipelineEvent.GetServiceBus().Configuration.HasIdempotenceTracker
                ||
                transportMessage == null)
            {
                return;
            }

            pipelineEvent.GetServiceBus().Configuration.IdempotenceTracker.Remove(transportMessage);

            if (log.IsTraceEnabled)
            {
                log.Trace(string.Format(ESBResources.TraceIdempotenceTrackerRemove, transportMessage.MessageType, transportMessage.MessageId));
            }
        }
    }
}
