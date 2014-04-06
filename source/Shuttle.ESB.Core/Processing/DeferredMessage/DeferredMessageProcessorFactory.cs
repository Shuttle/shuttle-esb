using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DeferredMessageProcessorFactory : IProcessorFactory
    {
		private readonly IServiceBus _bus;
	    private bool _instanced;

		public DeferredMessageProcessorFactory(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "_bus");

			_bus = bus;
		}

        public IProcessor Create()
        {
	        if (_instanced)
	        {
				throw new ProcessorException(ESBResources.DeferredMessageProcessorInstanceException);
	        }

	        _instanced = true;

	        return _bus.Configuration.DeferredMessageProcessor;
        }
    }
}