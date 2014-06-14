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

			State.SetWorkQueue(_bus.Configuration.Inbox.WorkQueue);
			State.SetDeferredQueue(_bus.Configuration.Inbox.DeferredQueue);
			State.SetErrorQueue(_bus.Configuration.Inbox.ErrorQueue);

			State.SetDurationToIgnoreOnFailure(_bus.Configuration.Inbox.DurationToIgnoreOnFailure);
			State.SetMaximumFailureCount(_bus.Configuration.Inbox.MaximumFailureCount);
		}
	}
}