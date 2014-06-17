namespace Shuttle.ESB.Core
{
    public class DistributorMessageObserver :
	    IPipelineObserver<OnHandleDistributeMessage>,
        IPipelineObserver<OnAbortPipeline>
    {
        public void Execute(OnHandleDistributeMessage pipelineEvent)
        {
			var state = pipelineEvent.Pipeline.State;
            var transportMessage = state.GetTransportMessage();

	        transportMessage.RecipientInboxWorkQueueUri = state.GetAvailableWorker().InboxWorkQueueUri;

			state.SetMessageSenderContext(new MessageSenderContext(transportMessage));
        }

        public void Execute(OnAbortPipeline pipelineEvent)
        {
			var state = pipelineEvent.Pipeline.State;

            state.GetServiceBus().Configuration
				.WorkerAvailabilityManager.ReturnAvailableWorker(state.GetAvailableWorker());
        }
    }
}