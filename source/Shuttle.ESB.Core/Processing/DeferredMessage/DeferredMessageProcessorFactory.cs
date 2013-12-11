using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DeferredMessageProcessorFactory : IProcessorFactory
    {
		private readonly IServiceBus bus;

		public DeferredMessageProcessorFactory(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			this.bus = bus;
		}

        public IProcessor Create()
        {
			return new DeferredMessageProcessor(bus);
        }
    }
}