using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public interface IWorkQueueConfiguration : IThreadCount
    {
        IQueue WorkQueue { get; }
    }
}