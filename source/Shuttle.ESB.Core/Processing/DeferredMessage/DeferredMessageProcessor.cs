namespace Shuttle.ESB.Core
{
	public class DeferredMessageProcessor : QueueProcessor<DeferredMessagePipeline>
	{
		public DeferredMessageProcessor(IServiceBus bus)
			: base(bus, bus.Configuration.ThreadActivityFactory.CreateDeferredMessageThreadActivity(bus))
		{
		}
	}
}

