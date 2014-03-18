using System;
using System.Threading;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DeferredMessageProcessor : IProcessor
	{
		private readonly IServiceBus _bus;

		public DeferredMessageProcessor(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			_bus = bus;
		}

		public void Execute(IActiveState state)
		{
			if (!_bus.Configuration.Inbox.ShouldProcessDeferred())
			{
				ThreadSleep.While(1000, state);

				return;
			}

			var pipeline = _bus.Configuration.PipelineFactory.GetPipeline<DeferredMessagePipeline>(_bus);


		}
	}
}

