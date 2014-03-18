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

			SetWorkQueue(_bus.Configuration.ControlInbox.WorkQueue);
			SetErrorQueue(_bus.Configuration.ControlInbox.ErrorQueue);

			SetDurationToIgnoreOnFailure(_bus.Configuration.ControlInbox.DurationToIgnoreOnFailure);
			SetMaximumFailureCount(_bus.Configuration.ControlInbox.MaximumFailureCount);
		}
	}
}