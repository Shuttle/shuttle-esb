using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class ProcessDeferredMessageObserver : IPipelineObserver<OnProcessDeferredMessage>
    {
		private readonly ILog _log;

		public ProcessDeferredMessageObserver()
		{
			_log = Log.For(this);
		}

		public void Execute(OnProcessDeferredMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var transportMessage = state.GetTransportMessage();
			var checkpointMessageId = state.GetCheckpointMessageId();

			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNull(state.GetTransportMessageStream(), "transportMessageStream");
			Guard.AgainstNull(state.GetWorkQueue(), "workQueue");
			Guard.AgainstNull(state.GetDeferredQueue(), "deferredQueue");

			if (transportMessage.IsIgnoring())
			{
				if (Guid.Empty.Equals(checkpointMessageId))
				{
					state.SetCheckpointMessageId(transportMessage.MessageId);
					state.SetNextDeferredProcessDate(transportMessage.IgnoreTillDate);
				}

				state.GetDeferredQueue().Release(transportMessage.MessageId);

				state.SetDeferredMessageReturned(false);

				return;
			}

			state.GetWorkQueue().Enqueue(transportMessage.MessageId, state.GetTransportMessageStream());			
			state.GetDeferredQueue().Acknowledge(transportMessage.MessageId);

			if (checkpointMessageId.Equals(transportMessage.MessageId))
			{
				state.SetCheckpointMessageId(Guid.Empty);
			}

			state.SetDeferredMessageReturned(true);

			if (_log.IsTraceEnabled)
			{
				_log.Trace(string.Format(ESBResources.TraceDeferredTransportMessageReturned, transportMessage.MessageId));
			}
		}
    }
}