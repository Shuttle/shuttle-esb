using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public interface IOutboxQueueConfiguration :
        IWorkQueueConfiguration,
        IErrorQueueConfiguration,
        IMessageFailureConfiguration,
        IThreadActivityConfiguration
    {
    }
}