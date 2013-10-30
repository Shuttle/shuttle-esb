using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class SendOutboxMessageObserver : IPipelineObserver<OnSendMessage>
	{
		private readonly ILog log;

		public SendOutboxMessageObserver()
		{
			log = Log.For(this);
		}

		public void Execute(OnSendMessage pipelineEvent)
		{
			var transportMessage = pipelineEvent.GetTransportMessage();

			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNullOrEmptyString(transportMessage.RecipientInboxWorkQueueUri, "uri");

			var queue =
					QueueManager.Instance.GetQueue(transportMessage.RecipientInboxWorkQueueUri);

			if (log.IsVerboseEnabled)
			{
				log.Verbose(string.Format(ESBResources.EnqueueMessage,
																	transportMessage.MessageType,
																	transportMessage.MessageId,
																	queue.Uri));
			}

			pipelineEvent.GetServiceBus().Events.OnBeforeEnqueueStream(
			this,
			new QueueMessageEventArgs(pipelineEvent, queue, transportMessage));

			using (var stream = pipelineEvent.GetTransportMessageStream().Copy())
			{
				queue.Enqueue(transportMessage.MessageId, stream);
			}

			pipelineEvent.GetServiceBus().Events.OnAfterEnqueueStream(
				this,
				new QueueMessageEventArgs(pipelineEvent, queue, transportMessage));
		}
	}
}