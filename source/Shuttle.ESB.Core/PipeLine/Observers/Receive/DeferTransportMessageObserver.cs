using System;
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
			Guard.AgainstNull(pipelineEvent.GetTransportMessageStream(), "transportMessage");
			Guard.AgainstNull(pipelineEvent.GetTransportMessage(), "transportMessage");
			Guard.AgainstNull(pipelineEvent.GetWorkQueue(), "workQueue");

			var transportMessage = pipelineEvent.GetTransportMessage();

		    if (!transportMessage.IsIgnoring())
			{
				return;
			}

			using (var stream = pipelineEvent.GetTransportMessageStream().Copy())
			{
				if (pipelineEvent.GetDeferredQueue() == null)
				{
					pipelineEvent.GetWorkQueue().Enqueue(transportMessage.MessageId, stream);
				}
				else
				{
					pipelineEvent.GetDeferredQueue().Enqueue(transportMessage.MessageId, stream);

					pipelineEvent.GetServiceBus().Configuration.Inbox.MessageDeferred(transportMessage.IgnoreTillDate);
				}
			}

			pipelineEvent.Pipeline.Abort();

            if (_log.IsVerboseEnabled)
            {
                _log.Verbose(string.Format(ESBResources.TransportMessageDeferred, transportMessage.MessageId,
                                          transportMessage.IgnoreTillDate.ToString(ESBResources.FormatDateFull)));
            }
		}
	}
}