using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DequeueWorkMessageObserver : IPipelineObserver<OnGetMessage>
    {
		private readonly ILog _log;

		public DequeueWorkMessageObserver()
		{
			_log = Log.For(this);
		}

        public void Execute(OnGetMessage pipelineEvent)
        {
            var queue = pipelineEvent.GetWorkQueue();

			Guard.AgainstNull(queue, "workQueue");

        	var bus = pipelineEvent.GetServiceBus();

			bus.Events.OnBeforeDequeueMessage(this, new BeforeDequeueEventArgs(pipelineEvent, queue));

            var stream = queue.GetMessage();

            // Abort the pipeline if there is no message on the queue
            if (stream == null)
            {
				pipelineEvent.GetServiceBus().Events.OnQueueEmpty(this, new QueueEmptyEventArgs(pipelineEvent, queue));
                pipelineEvent.Pipeline.Abort();
            }
            else
            {
				bus.Events.OnAfterDequeueStream(this, new QueueStreamEventArgs(pipelineEvent, queue, stream.Copy()));

				pipelineEvent.SetTransportMessageStream(stream);

                if (_log.IsVerboseEnabled)
                {
                    _log.Verbose(string.Format(ESBResources.DequeueStream, queue.Uri.Secured()));
                }
            }
        }
    }
}