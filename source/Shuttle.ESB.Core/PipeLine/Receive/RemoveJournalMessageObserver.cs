using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class RemoveJournalMessageObserver : IPipelineObserver<OnRemoveJournalMessage>
	{
		public void Execute(OnRemoveJournalMessage pipelineEvent)
		{
			var bus = pipelineEvent.GetServiceBus();
			var queue = pipelineEvent.GetJournalQueue();
			var transportMessage = pipelineEvent.GetTransportMessage();

			Log.Debug(string.Format(Resources.DebugRemoveMessageFromQueue,
			                        transportMessage.Message.GetType().FullName,
			                        transportMessage.MessageId,
			                        queue.Uri));

			bus.Events.OnBeforeRemoveMessage(this, new QueueMessageEventArgs(queue, transportMessage));
			queue.Remove(transportMessage.MessageId);
			bus.Events.OnAfterRemoveMessage(this, new QueueMessageEventArgs(queue, transportMessage));
		}
	}
}