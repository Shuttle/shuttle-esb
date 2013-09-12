using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DefaultThreadActivityFactory : IThreadActivityFactory
    {
        public IThreadActivity CreateInboxThreadActivity(IServiceBus bus)
        {
            var threadActivity = new ThreadActivity(bus.Configuration.Inbox);

            return bus.Configuration.IsWorker
                       ? (IThreadActivity) new WorkerThreadActivity(bus, threadActivity)
                       : threadActivity;
        }

        public IThreadActivity CreateControlInboxThreadActivity(IServiceBus bus)
        {
            return new ThreadActivity(bus.Configuration.ControlInbox);
        }

        public IThreadActivity CreateOutboxThreadActivity(IServiceBus bus)
        {
            return new ThreadActivity(bus.Configuration.Outbox);
        }
    }
}