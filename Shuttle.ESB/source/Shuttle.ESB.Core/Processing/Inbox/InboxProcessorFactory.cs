using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class InboxProcessorFactory : IProcessorFactory
    {
        private readonly IServiceBus bus;

        public InboxProcessorFactory(IServiceBus bus)
        {
            Guard.AgainstNull(bus, "bus");

            this.bus = bus;
        }

        public IProcessor Create()
        {
            return new InboxProcessor(bus, bus.Configuration.ThreadActivityFactory.CreateInboxThreadActivity(bus));
        }
    }
}