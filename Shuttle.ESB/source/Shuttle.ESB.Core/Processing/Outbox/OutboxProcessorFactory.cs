using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class OutboxProcessorFactory : IProcessorFactory
    {
		private readonly IServiceBus bus;

		public OutboxProcessorFactory(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			this.bus = bus;
		}

        public IProcessor Create()
        {
            return new OutboxProcessor(bus);
        }
    }
}