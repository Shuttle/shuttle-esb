namespace Shuttle.ESB.Core
{
	public class InboxMessagePipeline : ReceiveMessagePipeline
	{
		public InboxMessagePipeline(IServiceBus bus) : base(bus)
		{
        }

		public override sealed void Obtained()
		{
			base.Obtained();

			WorkQueue = _bus.Configuration.Inbox.WorkQueue;
			ErrorQueue = _bus.Configuration.Inbox.ErrorQueue;

			DurationToIgnoreOnFailure = _bus.Configuration.Inbox.DurationToIgnoreOnFailure;
			MaximumFailureCount = _bus.Configuration.Inbox.MaximumFailureCount;
		}
	}
}