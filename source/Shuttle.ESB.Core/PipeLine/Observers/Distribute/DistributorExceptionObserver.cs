using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DistributorExceptionObserver :
        IPipelineObserver<OnPipelineException>
    {
        public void Execute(OnPipelineException pipelineEvent)
        {
            var bus = pipelineEvent.GetServiceBus();

            bus.Events.OnBeforePipelineExceptionHandled(this, new PipelineExceptionEventArgs(pipelineEvent.Pipeline));

            try
            {
                if (pipelineEvent.Pipeline.ExceptionHandled)
                {
                    return;
                }

                var transportMessage = pipelineEvent.GetTransportMessage();

                if (transportMessage == null)
                {
                    return;
                }

                var action = bus.Configuration.Policy.EvaluateMessageDistributionFailure(pipelineEvent);

                transportMessage.RegisterFailure(pipelineEvent.Pipeline.Exception.CompactMessages(), action.TimeSpanToIgnoreRetriedMessage);

                if (action.Retry)
                {
                    pipelineEvent.GetWorkQueue().Enqueue(
                        transportMessage.MessageId,
                        pipelineEvent.GetServiceBus().Configuration.Serializer.Serialize(transportMessage));
                }
                else
                {
                    pipelineEvent.GetErrorQueue().Enqueue(
                        transportMessage.MessageId,
                        pipelineEvent.GetServiceBus().Configuration.Serializer.Serialize(transportMessage));
                }

                pipelineEvent.GetTransactionScope().Complete();
                pipelineEvent.GetTransactionScope().Dispose();

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