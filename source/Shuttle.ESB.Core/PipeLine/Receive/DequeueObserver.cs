using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DequeueObserver : IPipelineObserver<OnDequeue>
    {
        public void Execute(OnDequeue pipelineEvent)
        {
            var queue = pipelineEvent.GetWorkQueue();

			Guard.AgainstNull(queue, "queue");

        	var bus = pipelineEvent.GetServiceBus();

			bus.Events.OnBeforeDequeueMessage(this, new BeforeDequeueEventArgs(queue));
            var stream = queue.Dequeue();

            // Abort the pipeline if there is no message on the queue
            if (stream == null)
            {
				pipelineEvent.GetServiceBus().Events.OnQueueEmpty(this, new QueueEmptyEventArgs(queue));
				pipelineEvent.Abort();
            }
            else
            {
				bus.Events.OnAfterDequeueMessage(this, new QueueStreamEventArgs(queue, stream.Copy()));

				pipelineEvent.SetTransportMessageStream(stream);

                Log.Debug(string.Format(Resources.DebugDequeueStream, queue.Uri));
            }
        }
    }
}