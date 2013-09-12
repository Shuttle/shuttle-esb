using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class SendMessageObserver : IPipelineObserver<OnSendMessage>
	{
		public void Execute(OnSendMessage pipelineEvent)
		{
			var transportMessage = pipelineEvent.GetTransportMessage();

			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNullOrEmptyString(transportMessage.RecipientInboxWorkQueueUri, "uri");

			var bus = pipelineEvent.GetServiceBus();

			var queue = !bus.Configuration.HasOutbox
							? bus.Configuration.Queues.GetQueue(transportMessage.RecipientInboxWorkQueueUri)
							: bus.Configuration.Outbox.WorkQueue;

			Log.Debug(string.Format(Resources.DebugEnqueueMessage,
									transportMessage.Message.GetType().FullName,
									transportMessage.MessageId,
									queue.Uri));

			bus.Events.OnBeforeEnqueueMessage(this, new QueueMessageEventArgs(queue, transportMessage));

			queue.Enqueue(transportMessage.MessageId, pipelineEvent.GetTransportMessageStream().Copy());

			bus.Events.OnAfterEnqueueMessage(this, new QueueMessageEventArgs(queue, transportMessage));
		}
	}
}