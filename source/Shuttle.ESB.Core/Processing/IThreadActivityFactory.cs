using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public interface IThreadActivityFactory
    {
        IThreadActivity CreateInboxThreadActivity(IServiceBus bus);
        IThreadActivity CreateControlInboxThreadActivity(IServiceBus bus);
        IThreadActivity CreateOutboxThreadActivity(IServiceBus bus);
	    IThreadActivity CreateDeferredMessageThreadActivity(IServiceBus bus);
    }
}