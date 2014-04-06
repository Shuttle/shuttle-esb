namespace Shuttle.ESB.Core
{
	public class DeferredMessagePipeline : MessagePipeline
	{
		public DeferredMessagePipeline(IServiceBus bus)
			: base(bus)
		{
			RegisterStage("Process")
				.WithEvent<OnGetMessage>()
				.WithEvent<OnDeserializeTransportMessage>()
				.WithEvent<OnAfterDeserializeTransportMessage>()
				.WithEvent<OnProcessDeferredMessage>()
				.WithEvent<OnAfterProcessDeferredMessage>();

			RegisterObserver(new GetDeferredMessageObserver());
			RegisterObserver(new DeserializeTransportMessageObserver());
			RegisterObserver(new ProcessDeferredMessageObserver());
		}

		public override void Obtained()
		{
			base.Obtained();

			State.SetWorkQueue(_bus.Configuration.Inbox.WorkQueue);
			State.SetErrorQueue(_bus.Configuration.Inbox.ErrorQueue);
			State.SetDeferredQueue(_bus.Configuration.Inbox.DeferredQueue);
		}
	}
}