using System;

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

			RegisterObserver(new DequeueDeferredMessageObserver());
			RegisterObserver(new DeserializeTransportMessageObserver());
			RegisterObserver(new ProcessDeferredMessageObserver());
		}

		public override void Obtained()
		{
			base.Obtained();

			SetWorkQueue(_bus.Configuration.Inbox.WorkQueue);
			SetErrorQueue(_bus.Configuration.Inbox.ErrorQueue);
			SetDeferredQueue(_bus.Configuration.Inbox.DeferredQueue);
		}

		public bool Execute(Guid checkpointMessageId, DateTime nextDeferredProcessDate)
		{
			SetCheckpointMessageId(checkpointMessageId);
			SetNextDeferredProcessDate(nextDeferredProcessDate);
			SetDeferredMessageReturned(false);

			return Execute();
		}
	}
}