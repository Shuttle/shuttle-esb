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

	        var receivedMessage = queue.GetMessage();

            // Abort the pipeline if there is no message on the queue
            if (receivedMessage == null)
            {
				state.GetServiceBus().Events.OnQueueEmpty(this, new QueueEmptyEventArgs(pipelineEvent, queue));
                pipelineEvent.Pipeline.Abort();
            }
            else
            {
				state.SetWorking();
				state.SetReceivedMessage(receivedMessage);

                if (_log.IsVerboseEnabled)
                {
                    _log.Trace(string.Format(ESBResources.TraceStreamDequeued, queue.Uri.Secured()));
                }
            }
        }
    }
}