using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public interface IServiceBusTransactionScopeFactory
    {
        ServiceBusTransactionScope Create();
        ServiceBusTransactionScope Create(object message);
        ServiceBusTransactionScope Create(PipelineEvent pipelineEvent);
    }
}