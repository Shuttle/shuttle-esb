namespace Shuttle.ESB.Core
{
	public class WorkerAvailableHandler : IMessageHandler<WorkerThreadAvailableCommand>
	{
		public void ProcessMessage(HandlerContext<WorkerThreadAvailableCommand> context)
		{
			var distributeSendCount = context.Configuration.Inbox.DistributeSendCount > 0
									   ? context.Configuration.Inbox.DistributeSendCount
									   : 5;

			for (var i = 0; i < distributeSendCount; i++)
			{
				context.Configuration.WorkerAvailabilityManager.WorkerAvailable(context.Message);
			}
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}