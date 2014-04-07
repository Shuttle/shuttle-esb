using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class GetDeferredMessageObserver : IPipelineObserver<OnGetMessage>
    {
		private readonly ILog _log;

		public GetDeferredMessageObserver()
		{
			_log = Log.For(this);
		}

        public void Execute(OnGetMessage pipelineEvent)
        {
			var state = pipelineEvent.Pipeline.State;
			var queue = state.GetDeferredQueue();

			Guard.AgainstNull(queue, "deferredQueue");

        	var bus = state.GetServiceBus();

            var receivedMessage = queue.GetMessage();

            // Abort the pipeline if there is no message on the queue
            if (receivedMessage == null)
            {
				state.SetNextDeferredProcessDate(DateTime.MaxValue);
				state.GetServiceBus().Events.OnQueueEmpty(this, new QueueEmptyEventArgs(pipelineEvent, queue));
                pipelineEvent.Pipeline.Abort();
            }
            else
            {
				state.SetWorking();
				state.SetReceivedMessage(receivedMessage);

                if (_log.IsVerboseEnabled)
                {
                    _log.Verbose(string.Format(ESBResources.DequeueStream, queue.Uri.Secured()));
                }
            }
        }
    }
}