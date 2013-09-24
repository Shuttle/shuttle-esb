namespace Shuttle.ESB.Core
{
    public interface IJournalQueueConfiguration
    {
        IQueue JournalQueue { get; }
    }
}