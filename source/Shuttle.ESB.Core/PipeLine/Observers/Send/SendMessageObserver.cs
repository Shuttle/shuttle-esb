using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class SendMessageObserver : IPipelineObserver<OnSendMessage>
	{
		private readonly ILog log;

		public SendMessageObserver()
		{
			log = Log.For(this);
		}

		public void Execute(OnSendMessage pipelineEvent)
		{
			var transportMessage = pipelineEvent.GetTransportMessage();

			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNullOrEmptyString(transportMessage.RecipientInboxWorkQueueUri, "uri");

			var bus = pipelineEvent.GetServiceBus();

			if (transportMessage.IsIgnoring() && bus.Configuration.HasDeferredMessageManager)
			{
				bus.Configuration.DeferredMessageManager.Register(transportMessage.IgnoreTillDate, pipelineEvent.GetTransportMessageStream());

				return;
			}

			var queue = !bus.Configuration.HasOutbox
						? QueueManager.Instance.GetQueue(transportMessage.RecipientInboxWorkQueueUri)
						: bus.Configuration.Outbox.WorkQueue;

			if (log.IsVerboseEnabled)
			{
				log.Verbose(string.Format(ESBResources.TraceCorrelationIdReceived, transportMessage.CorrelationId));

				foreach (var header in transportMessage.Headers)
				{
					log.Verbose(string.Format(ESBResources.TraceTransportHeaderReceived, header.Key, header.Value));
				}

				log.Verbose(string.Format(ESBResources.EnqueueMessage,
										  transportMessage.MessageType,
										  transportMessage.MessageId,
										  queue.Uri));
			}

			bus.Events.OnBeforeEnqueueStream(this, new QueueMessageEventArgs(pipelineEvent, queue, transportMessage));

			using (var stream = pipelineEvent.GetTransportMessageStream().Copy())
			{
				queue.Enqueue(transportMessage.MessageId, stream);
			}

			bus.Events.OnAfterEnqueueStream(this, new QueueMessageEventArgs(pipelineEvent, queue, transportMessage));
		}
	}
}