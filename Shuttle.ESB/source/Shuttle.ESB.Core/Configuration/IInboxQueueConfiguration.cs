namespace Shuttle.ESB.Core
{
    public interface IInboxQueueConfiguration : IWorkProcessorConfiguration
    {
        QueueStartupAction WorkQueueStartupAction { get; set; }
        bool Distribute { get; set; }
    }
}