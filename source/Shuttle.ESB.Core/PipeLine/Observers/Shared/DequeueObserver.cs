using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DequeueObserver : IPipelineObserver<OnDequeue>
    {
		private readonly ILog log;

		public DequeueObserver()
		{
			log = Log.For(this);
		}

        public void Execute(OnDequeue pipelineEvent)
        {
            var queue = pipelineEvent.GetWorkQueue();

			Guard.AgainstNull(queue, "queue");

        	var bus = pipelineEvent.GetServiceBus();

			bus.Events.OnBeforeDequeueMessage(this, new BeforeDequeueEventArgs(pipelineEvent, queue));

            var stream = queue.Dequeue();

            // Abort the pipeline if there is no message on the queue
            if (stream == null)
            {
				pipelineEvent.GetServiceBus().Events.OnQueueEmpty(this, new QueueEmptyEventArgs(pipelineEvent, queue));
                pipelineEvent.SetTransactionComplete();
                pipelineEvent.Pipeline.Abort();
            }
            else
            {
				bus.Events.OnAfterDequeueStream(this, new QueueStreamEventArgs(pipelineEvent, queue, stream.Copy()));

				pipelineEvent.SetTransportMessageStream(stream);

                if (log.IsVerboseEnabled)
                {
                    log.Verbose(string.Format(ESBResources.DequeueStream, queue.Uri));
                }
            }
        }
    }
}