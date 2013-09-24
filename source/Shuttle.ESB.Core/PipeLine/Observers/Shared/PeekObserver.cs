using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class PeekObserver : IPipelineObserver<OnPeek>
    {
        public void Execute(OnPeek pipelineEvent)
        {
            var queue = pipelineEvent.GetWorkQueue();

			Guard.AgainstNull(queue, "queue");

            if (!queue.IsEmpty())
            {
                return;
            }

            pipelineEvent.GetServiceBus().Events.OnQueueEmpty(this, new QueueEmptyEventArgs(pipelineEvent, queue));
            pipelineEvent.Pipeline.Abort();
        }
    }
}