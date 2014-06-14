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

                var transportMessage = state.GetTransportMessage();

                if (transportMessage == null)
                {
                    return;
                }

                var action = bus.Configuration.Policy.EvaluateMessageDistributionFailure(pipelineEvent);

                transportMessage.RegisterFailure(pipelineEvent.Pipeline.Exception.CompactMessages(), action.TimeSpanToIgnoreRetriedMessage);

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

                state.GetTransactionScope().Complete();
                state.GetTransactionScope().Dispose();

                pipelineEvent.Pipeline.MarkExceptionHandled();
                bus.Events.OnAfterPipelineExceptionHandled(this, new PipelineExceptionEventArgs(pipelineEvent.Pipeline));
            }
            finally
            {
                pipelineEvent.Pipeline.Abort();
            }
        }
    }
}