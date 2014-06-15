namespace Shuttle.ESB.Core
{
    public interface IInboxQueueConfiguration : IWorkProcessorConfiguration
    {
	    bool Distribute { get; set; }
		IQueue DeferredQueue { get; set; }
    }
}