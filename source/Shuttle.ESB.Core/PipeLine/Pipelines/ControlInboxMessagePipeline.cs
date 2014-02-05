namespace Shuttle.ESB.Core
{
	public class ControlInboxMessagePipeline : ReceiveMessagePipeline
	{
		public ControlInboxMessagePipeline(IServiceBus bus) : base(bus)
		{
        }

		public override sealed void Obtained()
		{
			base.Obtained();

			WorkQueue = _bus.Configuration.ControlInbox.WorkQueue;
			ErrorQueue = _bus.Configuration.ControlInbox.ErrorQueue;

			DurationToIgnoreOnFailure = _bus.Configuration.ControlInbox.DurationToIgnoreOnFailure;
			MaximumFailureCount = _bus.Configuration.ControlInbox.MaximumFailureCount;
		}
	}
}