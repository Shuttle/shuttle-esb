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
			Guard.AgainstNull(state.GetTransportMessageStream(), "transportMessage");
			Guard.AgainstNull(state.GetTransportMessage(), "transportMessage");
			Guard.AgainstNull(state.GetWorkQueue(), "workQueue");

			var transportMessage = state.GetTransportMessage();

			if (!transportMessage.IsIgnoring())
			{
				return;
			}

			using (var stream = state.GetTransportMessageStream().Copy())
			{
				if (state.GetDeferredQueue() == null)
				{
					state.GetWorkQueue().Enqueue(transportMessage.MessageId, stream);
				}
				else
				{
					state.GetDeferredQueue().Enqueue(transportMessage.MessageId, stream);

					state.GetServiceBus().Configuration.DeferredMessageProcessor.MessageDeferred(transportMessage.IgnoreTillDate);
				}
			}

			state.GetWorkQueue().Acknowledge(transportMessage.MessageId);

			if (_log.IsTraceEnabled)
			{
				_log.Trace(string.Format(ESBResources.TraceTransportMessageDeferred, transportMessage.MessageId,
				                         transportMessage.IgnoreTillDate.ToString(ESBResources.FormatDateFull)));
			}

			pipelineEvent.Pipeline.Abort();
		}
	}
}