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
			var receivedMessage = state.GetReceivedMessage();
			var workQueue = state.GetWorkQueue();
			var deferredQueue = state.GetDeferredQueue();

			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNull(receivedMessage, "receivedMessage");
			Guard.AgainstNull(workQueue, "workQueue");
			Guard.AgainstNull(deferredQueue, "deferredQueue");

			if (transportMessage.IsIgnoring())
			{
				if (Guid.Empty.Equals(checkpointMessageId))
				{
					state.SetCheckpointMessageId(transportMessage.MessageId);
					state.SetNextDeferredProcessDate(transportMessage.IgnoreTillDate);
				}

				deferredQueue.Release(receivedMessage.AcknowledgementToken);

				state.SetDeferredMessageReturned(false);

				return;
			}

			workQueue.Enqueue(transportMessage.MessageId, receivedMessage.Stream);			
			deferredQueue.Acknowledge(receivedMessage.AcknowledgementToken);

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