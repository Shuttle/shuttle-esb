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

			State.SetWorkQueue(_bus.Configuration.ControlInbox.WorkQueue);
			State.SetErrorQueue(_bus.Configuration.ControlInbox.ErrorQueue);

			State.SetDurationToIgnoreOnFailure(_bus.Configuration.ControlInbox.DurationToIgnoreOnFailure);
			State.SetMaximumFailureCount(_bus.Configuration.ControlInbox.MaximumFailureCount);
		}
	}
}