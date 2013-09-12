using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DistributorMessageObserver :
		IPipelineObserver<OnGetAvilableWorker>,
		IPipelineObserver<OnHandleDistributeMessage>,
		IPipelineObserver<OnAbortPipeline>
	{
		public void Execute(OnGetAvilableWorker pipelineEvent)
		{
			var availableWorker = pipelineEvent.GetServiceBus().Configuration
					.WorkerAvailabilityManager.GetAvailableWorker();

			if (availableWorker == null)
			{
				pipelineEvent.Abort();

				return;
			}

			pipelineEvent.SetAvailableWorker(availableWorker);
		}

		public void Execute(OnHandleDistributeMessage pipelineEvent)
		{
			pipelineEvent.GetTransportMessage().RecipientInboxWorkQueueUri =
				pipelineEvent.GetAvailableWorker().InboxWorkQueueUri;
		}

		public void Execute(OnAbortPipeline pipelineEvent)
		{
			pipelineEvent.GetServiceBus().Configuration.WorkerAvailabilityManager
				.ReturnAvailableWorker(pipelineEvent.GetAvailableWorker());
		}
	}
}