using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public interface IServiceBusTransactionScopeFactory
    {
        IServiceBusTransactionScope Create(PipelineEvent pipelineEvent);
    }
}