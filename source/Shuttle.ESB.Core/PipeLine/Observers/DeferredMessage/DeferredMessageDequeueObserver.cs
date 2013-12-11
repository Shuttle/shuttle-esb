using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DeferredMessageDequeueObserver : IPipelineObserver<OnDequeue>
    {
	    public void Execute(OnDequeue pipelineEvent)
	    {
		    var bus = pipelineEvent.GetServiceBus();

			if (!bus.Configuration.HasDeferredMessageQueue)
		    {
				throw new DeferredMessageException(ESBResources.NoDeferredMessageQueue);
		    }

		    var queue = bus.Configuration.DeferredMessageConfiguration.DeferredMessageQueue;

            var stream = queue.Dequeue(DateTime.Now);

			if (stream == null)
            {
                pipelineEvent.SetTransactionComplete();
                pipelineEvent.Pipeline.Abort();
            }
            else
            {
				pipelineEvent.SetTransportMessageStream(stream);
            }
        }
    }
}