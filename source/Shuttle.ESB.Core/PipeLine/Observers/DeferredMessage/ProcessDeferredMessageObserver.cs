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
			var transportMessage = pipelineEvent.GetTransportMessage();
			var checkpointMessageId = pipelineEvent.GetCheckpointMessageId();

			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNull(pipelineEvent.GetTransportMessageStream(), "transportMessageStream");
			Guard.AgainstNull(pipelineEvent.GetWorkQueue(), "workQueue");
			Guard.AgainstNull(pipelineEvent.GetDeferredQueue(), "deferredQueue");

			if (transportMessage.IsIgnoring())
			{
				if (Guid.Empty.Equals(checkpointMessageId))
				{
					pipelineEvent.SetCheckpointMessageId(transportMessage.MessageId);
				}

				return;
			}

			pipelineEvent.GetWorkQueue().Enqueue(transportMessage.MessageId, pipelineEvent.GetTransportMessageStream());			
			pipelineEvent.GetDeferredQueue().Acknowledge(transportMessage.MessageId);

			if (checkpointMessageId.Equals(transportMessage.MessageId))
			{
				pipelineEvent.SetCheckpointMessageId(Guid.Empty);
			}
		}
    }
}