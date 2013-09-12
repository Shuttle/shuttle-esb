namespace Shuttle.ESB.Core
{
	public class ControlInboxMessagePipeline : ReceiveMessagePipeline
	{
		public ControlInboxMessagePipeline(IServiceBus bus) : base(bus, bus.Configuration.ControlInbox.HasJournalQueue)
		{
        }

		public override sealed void Obtained()
		{
			base.Obtained();

			WorkQueue = bus.Configuration.ControlInbox.WorkQueue;
			JournalQueue = bus.Configuration.ControlInbox.JournalQueue;
			ErrorQueue = bus.Configuration.ControlInbox.ErrorQueue;

			DurationToIgnoreOnFailure = bus.Configuration.ControlInbox.DurationToIgnoreOnFailure;
			MaximumFailureCount = bus.Configuration.ControlInbox.MaximumFailureCount;
		}
	}
}