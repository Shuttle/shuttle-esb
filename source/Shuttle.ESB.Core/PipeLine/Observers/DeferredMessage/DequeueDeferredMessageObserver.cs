using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DequeueDeferredMessageObserver : IPipelineObserver<OnGetMessage>
    {
		private readonly ILog _log;

		public DequeueDeferredMessageObserver()
		{
			_log = Log.For(this);
		}

        public void Execute(OnGetMessage pipelineEvent)
        {
            var queue = pipelineEvent.GetDeferredQueue();

			Guard.AgainstNull(queue, "deferredQueue");

        	var bus = pipelineEvent.GetServiceBus();

            var stream = queue.GetMessage();

            // Abort the pipeline if there is no message on the queue
            if (stream == null)
            {
				pipelineEvent.SetNextDeferredProcessDate(DateTime.MaxValue);
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