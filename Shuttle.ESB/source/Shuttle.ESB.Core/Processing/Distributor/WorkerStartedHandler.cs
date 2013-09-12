namespace Shuttle.ESB.Core
{
	public class WorkerStartedHandler : IMessageHandler<WorkerStartedEvent>
	{
		public void ProcessMessage(HandlerContext<WorkerStartedEvent> context)
		{
			context.Bus.Configuration.WorkerAvailabilityManager.WorkerStarted(context.Message);
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}