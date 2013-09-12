using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class RemoveJournalMessageObserver : IPipelineObserver<OnRemoveJournalMessage>
	{
		private readonly ILog log;

		public RemoveJournalMessageObserver()
		{
			log = Log.For(this);
		}

		public void Execute(OnRemoveJournalMessage pipelineEvent)
		{
			var bus = pipelineEvent.GetServiceBus();
			var queue = pipelineEvent.GetJournalQueue();
			var transportMessage = pipelineEvent.GetTransportMessage();

            if (log.IsVerboseEnabled)
            {
                log.Verbose(string.Format(ESBResources.RemoveMessageFromQueue,
                                          transportMessage.MessageType,
                                          transportMessage.MessageId,
                                          queue.Uri));
            }

		    bus.Events.OnBeforeRemoveMessage(this, new QueueMessageEventArgs(pipelineEvent, queue, transportMessage));
			queue.Remove(transportMessage.MessageId);
			bus.Events.OnAfterRemoveMessage(this, new QueueMessageEventArgs(pipelineEvent, queue, transportMessage));
		}
	}
}