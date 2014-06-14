using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class SendMessageObserver : IPipelineObserver<OnSendMessage>
	{
		private readonly ILog _log;

		public SendMessageObserver()
		{
			_log = Log.For(this);
		}

		public void Execute(OnSendMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var bus = state.GetServiceBus();

			if (bus.IsHandlingTransportMessage && bus.Configuration.HasIdempotenceService)
			{
				try
				{
					bus.Configuration.IdempotenceService.AddDeferredMessage(bus.TransportMessageBeingHandled, state.GetTransportMessageStream());
				}
				catch (Exception ex)
				{
					bus.Configuration.IdempotenceService.AccessException(_log, ex, pipelineEvent.Pipeline);
				}

				return;
			}

			var transportMessage = state.GetTransportMessage();

			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNullOrEmptyString(transportMessage.RecipientInboxWorkQueueUri, "uri");

			if (transportMessage.IsIgnoring() && bus.Configuration.HasDeferredQueue)
			{
				bus.Configuration.Inbox.DeferredQueue.Enqueue(transportMessage.MessageId, state.GetTransportMessageStream());

				return;
			}

			var queue = !bus.Configuration.HasOutbox
						? bus.Configuration.QueueManager.GetQueue(transportMessage.RecipientInboxWorkQueueUri)
						: bus.Configuration.Outbox.WorkQueue;

			if (_log.IsVerboseEnabled)
			{
				_log.Verbose(string.Format(ESBResources.TraceCorrelationIdReceived, transportMessage.CorrelationId));

				foreach (var header in transportMessage.Headers)
				{
					_log.Verbose(string.Format(ESBResources.TraceTransportHeaderReceived, header.Key, header.Value));
				}

				_log.Verbose(string.Format(ESBResources.EnqueueMessage,
										  transportMessage.MessageType,
										  transportMessage.MessageId,
										  queue.Uri));
			}

			using (var stream = state.GetTransportMessageStream().Copy())
			{
				queue.Enqueue(transportMessage.MessageId, stream);
			}
		}
	}
}