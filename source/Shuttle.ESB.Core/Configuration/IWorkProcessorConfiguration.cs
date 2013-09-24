using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public interface IWorkProcessorConfiguration:
        IWorkQueueConfiguration,
        IJournalQueueConfiguration,
        IErrorQueueConfiguration,
        IMessageFailureConfiguration,
        IThreadActivityConfiguration
    {
        bool HasJournalQueue { get; }
    }
}