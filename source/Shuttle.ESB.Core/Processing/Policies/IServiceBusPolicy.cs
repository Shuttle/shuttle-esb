using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public interface IServiceBusPolicy
    {
        MessageFailureAction EvaluateMessageHandlingFailure(OnPipelineException pipelineEvent);
        MessageFailureAction EvaluateMessageDistributionFailure(OnPipelineException pipelineEvent);
        MessageFailureAction EvaluateOutboxFailure(OnPipelineException pipelineEvent);
    }
}