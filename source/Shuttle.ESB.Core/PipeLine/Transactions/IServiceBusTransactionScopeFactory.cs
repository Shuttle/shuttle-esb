using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public interface IServiceBusTransactionScopeFactory
    {
        IServiceBusTransactionScope Create();
        IServiceBusTransactionScope Create(object message);
        IServiceBusTransactionScope Create(PipelineEvent pipelineEvent);
    }
}