using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DeferTransportMessageObserver : IPipelineObserver<OnAfterDeserializeTransportMessage>
	{
		private readonly ILog _log;

		public DeferTransportMessageObserver()
		{
			_log = Log.For(this);
		}

		public void Execute(OnAfterDeserializeTransportMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var receivedMessage = state.GetReceivedMessage();
			var transportMessage = state.GetTransportMessage();
			var workQueue = state.GetWorkQueue();

			Guard.AgainstNull(receivedMessage, "receivedMessage");
			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNull(workQueue, "workQueue");

			if (!transportMessage.IsIgnoring())
			{
				return;
			}

			using (var stream = receivedMessage.Stream.Copy())
			{
				if (state.GetDeferredQueue() == null)
				{
					workQueue.Enqueue(transportMessage.MessageId, stream);
				}
				else
				{
					state.GetDeferredQueue().Enqueue(transportMessage.MessageId, stream);

					state.GetServiceBus().Configuration.DeferredMessageProcessor.MessageDeferred(transportMessage.IgnoreTillDate);
				}
			}

			workQueue.Acknowledge(receivedMessage.AcknowledgementToken);

			if (_log.IsTraceEnabled)
			{
				_log.Trace(string.Format(ESBResources.TraceTransportMessageDeferred, transportMessage.MessageId,
				                         transportMessage.IgnoreTillDate.ToString(ESBResources.FormatDateFull)));
			}

			pipelineEvent.Pipeline.Abort();
		}
	}
}