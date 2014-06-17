namespace Shuttle.ESB.Core
{
	public class WorkerAvailableHandler : IMessageHandler<WorkerThreadAvailableCommand>
	{
		public void ProcessMessage(HandlerContext<WorkerThreadAvailableCommand> context)
		{
			context.Configuration.WorkerAvailabilityManager.WorkerAvailable(context.Message);
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}