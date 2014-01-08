using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DistributorMessageObserver :
        IPipelineObserver<OnHandleDistributeMessage>,
        IPipelineObserver<OnAbortPipeline>
    {
        public void Execute(OnHandleDistributeMessage pipelineEvent)
        {
            var bus = pipelineEvent.GetServiceBus();
            var destinationQueue = pipelineEvent.GetServiceBus().Configuration.QueueManager.GetQueue(pipelineEvent.GetAvailableWorker().InboxWorkQueueUri);
            var transportMessage = pipelineEvent.GetTransportMessage();

            bus.Events.OnBeforeDistributeMessage(
                this,
				new DistributeMessageEventArgs(pipelineEvent, destinationQueue, transportMessage));

            transportMessage.RecipientInboxWorkQueueUri = pipelineEvent.GetAvailableWorker().InboxWorkQueueUri;

            bus.Events.OnAfterDistributeMessage(
                this,
				new DistributeMessageEventArgs(pipelineEvent, destinationQueue, transportMessage));
        }

        public void Execute(OnAbortPipeline pipelineEvent)
        {
            pipelineEvent.GetServiceBus().Configuration.WorkerAvailabilityManager
                .ReturnAvailableWorker(pipelineEvent.GetAvailableWorker());
        }
    }
}