using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class GetWorkMessageObserver : IPipelineObserver<OnGetMessage>
    {
		private readonly ILog _log;

		public GetWorkMessageObserver()
		{
			_log = Log.For(this);
		}

        public void Execute(OnGetMessage pipelineEvent)
        {
			var state = pipelineEvent.Pipeline.State;
			var queue = state.GetWorkQueue();

			Guard.AgainstNull(queue, "workQueue");

        	var bus = state.GetServiceBus();

			bus.Events.OnBeforeDequeueMessage(this, new BeforeDequeueEventArgs(pipelineEvent, queue));

            var stream = queue.GetMessage();

            // Abort the pipeline if there is no message on the queue
            if (stream == null)
            {
				state.GetServiceBus().Events.OnQueueEmpty(this, new QueueEmptyEventArgs(pipelineEvent, queue));
                pipelineEvent.Pipeline.Abort();
            }
            else
            {
				bus.Events.OnAfterDequeueStream(this, new QueueStreamEventArgs(pipelineEvent, queue, stream.Copy()));

				state.SetWorking();
				state.SetTransportMessageStream(stream);

                if (_log.IsVerboseEnabled)
                {
                    _log.Verbose(string.Format(ESBResources.DequeueStream, queue.Uri.Secured()));
                }
            }
        }
    }
}