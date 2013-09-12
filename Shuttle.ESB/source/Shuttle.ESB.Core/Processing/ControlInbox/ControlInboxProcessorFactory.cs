using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class ControlInboxProcessorFactory : IProcessorFactory
    {
        private readonly IServiceBus bus;

        public ControlInboxProcessorFactory(IServiceBus bus)
        {
            Guard.AgainstNull(bus, "bus");

            this.bus = bus;
        }

        public IProcessor Create()
        {
            return new ControlInboxProcessor(bus);
        }
    }
}