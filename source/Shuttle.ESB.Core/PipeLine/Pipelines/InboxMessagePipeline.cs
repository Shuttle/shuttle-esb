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

			SetWorkQueue(_bus.Configuration.Inbox.WorkQueue);
			SetDeferredQueue(_bus.Configuration.Inbox.DeferredQueue);
			SetErrorQueue(_bus.Configuration.Inbox.ErrorQueue);

			SetDurationToIgnoreOnFailure(_bus.Configuration.Inbox.DurationToIgnoreOnFailure);
			SetMaximumFailureCount(_bus.Configuration.Inbox.MaximumFailureCount);
		}
	}
}