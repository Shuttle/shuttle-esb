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

			WorkQueue = bus.Configuration.Inbox.WorkQueue;
			JournalQueue = bus.Configuration.Inbox.JournalQueue;
			ErrorQueue = bus.Configuration.Inbox.ErrorQueue;

			DurationToIgnoreOnFailure = bus.Configuration.Inbox.DurationToIgnoreOnFailure;
			MaximumFailureCount = bus.Configuration.Inbox.MaximumFailureCount;
		}
	}
}