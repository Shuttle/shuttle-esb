using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DispatchTransportMessageObserver : IPipelineObserver<OnDispatchTransportMessage>
	{
		private readonly ILog _log;

		public DispatchTransportMessageObserver()
		{
			_log = Log.For(this);
		}

		public void Execute(OnDispatchTransportMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var messageSenderContext = state.GetMessageSenderContext();
			var bus = state.GetServiceBus();

			Guard.AgainstNull(messageSenderContext, "messageSenderContext");

			if (messageSenderContext.HasTransportMessageReceived && bus.Configuration.HasIdempotenceService)
			{
				try
				{
					bus.Configuration.IdempotenceService.AddDeferredMessage(messageSenderContext.TransportMessageReceived,
					                                                        state.GetTransportMessageStream());
				}
				catch (Exception ex)
				{
					bus.Configuration.IdempotenceService.AccessException(_log, ex, pipelineEvent.Pipeline);
				}

				return;
			}

			var transportMessage = messageSenderContext.TransportMessage;

			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNullOrEmptyString(transportMessage.RecipientInboxWorkQueueUri, "uri");

			if (transportMessage.IsIgnoring() && bus.Configuration.HasDeferredQueue)
			{
				bus.Configuration.Inbox.DeferredQueue.Enqueue(transportMessage.MessageId, state.GetTransportMessageStream());
				bus.Configuration.DeferredMessageProcessor.MessageDeferred(transportMessage.IgnoreTillDate);

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

				_log.Trace(string.Format(ESBResources.TraceMessageEnqueued,
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