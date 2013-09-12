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

			if (!transportMessage.Ignore())
			{
				using (var stream = pipelineEvent.GetTransportMessageStream().Copy())
				{
					pipelineEvent.GetJournalQueue().Enqueue(transportMessage.MessageId, stream);
				}
			}
			else
			{
				using (var stream = pipelineEvent.GetTransportMessageStream().Copy())
				{
					pipelineEvent.GetWorkQueue().Enqueue(transportMessage.MessageId, stream);
				}

				pipelineEvent.SetTransactionComplete();

				pipelineEvent.Abort();
			}
		}
	}
}