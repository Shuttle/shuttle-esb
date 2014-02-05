using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public interface IWorkProcessorConfiguration:
        IWorkQueueConfiguration,
        IErrorQueueConfiguration,
        IMessageFailureConfiguration,
        IThreadActivityConfiguration
    {
    }
}