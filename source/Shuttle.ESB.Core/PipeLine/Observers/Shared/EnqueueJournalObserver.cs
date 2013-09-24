using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class EnqueueJournalObserver : IPipelineObserver<OnEnqueueJournal>
	{
		public void Execute(OnEnqueueJournal pipelineEvent)
		{
			var transportMessage = pipelineEvent.GetTransportMessage();

			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNull(pipelineEvent.GetTransportMessageStream(), "stream");

			using (var stream = pipelineEvent.GetTransportMessageStream().Copy())
			{
				pipelineEvent.GetJournalQueue().Enqueue(transportMessage.MessageId, stream);
			}
		}
	}
}

