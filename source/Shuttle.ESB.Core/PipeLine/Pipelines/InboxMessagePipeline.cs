namespace Shuttle.ESB.Core
{
	public class InboxMessagePipeline : ReceiveMessagePipeline
	{
		public InboxMessagePipeline(IServiceBus bus) : base(bus, bus.Configuration.Inbox.HasJournalQueue)
		{
        }

		public override sealed void Obtained()
		{
			base.Obtained();

			WorkQueue = _bus.Configuration.Inbox.WorkQueue;
			JournalQueue = _bus.Configuration.Inbox.JournalQueue;
			ErrorQueue = _bus.Configuration.Inbox.ErrorQueue;

			DurationToIgnoreOnFailure = _bus.Configuration.Inbox.DurationToIgnoreOnFailure;
			MaximumFailureCount = _bus.Configuration.Inbox.MaximumFailureCount;
		}
	}
}