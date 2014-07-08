using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DistributorExceptionObserver :
        IPipelineObserver<OnPipelineException>
    {
        public void Execute(OnPipelineException pipelineEvent)
        {
			var state = pipelineEvent.Pipeline.State;
			var bus = state.GetServiceBus();

            bus.Events.OnBeforePipelineExceptionHandled(this, new PipelineExceptionEventArgs(pipelineEvent.Pipeline));

            try
            {
                if (pipelineEvent.Pipeline.ExceptionHandled)
                {
                    return;
                }

	            try
	            {
		            var transportMessage = state.GetTransportMessage();

		            if (transportMessage == null)
		            {
			            return;
		            }

		            var action = bus.Configuration.Policy.EvaluateMessageDistributionFailure(pipelineEvent);

		            transportMessage.RegisterFailure(pipelineEvent.Pipeline.Exception.AllMessages(),
		                                             action.TimeSpanToIgnoreRetriedMessage);

		            if (action.Retry)
		            {
			            state.GetWorkQueue().Enqueue(
				            transportMessage.MessageId,
				            state.GetServiceBus().Configuration.Serializer.Serialize(transportMessage));
		            }
		            else
		            {
			            state.GetErrorQueue().Enqueue(
				            transportMessage.MessageId,
				            state.GetServiceBus().Configuration.Serializer.Serialize(transportMessage));
		            }

		            state.GetWorkQueue().Acknowledge(state.GetReceivedMessage().AcknowledgementToken);
	            }
	            finally
	            {
		            pipelineEvent.Pipeline.MarkExceptionHandled();
		            bus.Events.OnAfterPipelineExceptionHandled(this, new PipelineExceptionEventArgs(pipelineEvent.Pipeline));
	            }
            }
            finally
            {
                pipelineEvent.Pipeline.Abort();
            }
        }
    }
}