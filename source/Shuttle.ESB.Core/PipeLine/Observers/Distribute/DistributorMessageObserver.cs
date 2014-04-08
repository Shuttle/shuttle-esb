using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DistributorMessageObserver :
        IPipelineObserver<OnHandleDistributeMessage>,
        IPipelineObserver<OnAbortPipeline>
    {
        public void Execute(OnHandleDistributeMessage pipelineEvent)
        {
			var state = pipelineEvent.Pipeline.State;
			var bus = state.GetServiceBus();
            var destinationQueue = state.GetServiceBus().Configuration.QueueManager.GetQueue(state.GetAvailableWorker().InboxWorkQueueUri);
            var transportMessage = state.GetTransportMessage();

	        transportMessage.RecipientInboxWorkQueueUri = state.GetAvailableWorker().InboxWorkQueueUri;
        }

        public void Execute(OnAbortPipeline pipelineEvent)
        {
			var state = pipelineEvent.Pipeline.State;

            state.GetServiceBus().Configuration
				.WorkerAvailabilityManager.ReturnAvailableWorker(state.GetAvailableWorker());
        }
    }
}