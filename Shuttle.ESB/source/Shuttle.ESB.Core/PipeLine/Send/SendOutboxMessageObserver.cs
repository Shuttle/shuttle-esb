using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class SendOutboxMessageObserver : IPipelineObserver<OnSendMessage>
    {
        public void Execute(OnSendMessage pipelineEvent)
        {
        	var transportMessage = pipelineEvent.GetTransportMessage();

        	Guard.AgainstNull(transportMessage, "transportMessage");
            Guard.AgainstNullOrEmptyString(transportMessage.RecipientInboxWorkQueueUri, "uri");

        	var queue = pipelineEvent.GetServiceBus().Configuration.Queues.GetQueue(transportMessage.RecipientInboxWorkQueueUri);

			Log.Debug(string.Format(Resources.DebugEnqueueMessage,
							transportMessage.Message.GetType().FullName,
							transportMessage.MessageId,
							queue.Uri));

			pipelineEvent.GetServiceBus().Events.OnBeforeEnqueueMessage(this, new QueueMessageEventArgs(queue, transportMessage));
        	
			using(var stream = pipelineEvent.GetTransportMessageStream().Copy())
        	{
        		queue.Enqueue(transportMessage.MessageId, stream);
        	}

			pipelineEvent.GetServiceBus().Events.OnAfterEnqueueMessage(this, new QueueMessageEventArgs(queue, transportMessage));
		}
    }
}