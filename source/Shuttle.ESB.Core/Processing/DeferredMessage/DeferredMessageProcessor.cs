using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DeferredMessageProcessor : IProcessor
	{
		private readonly IServiceBus _bus;

		private Guid _checkpointMessageId = Guid.Empty;
		private DateTime _nextDeferredProcessDate = DateTime.MinValue;

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

			var pipeline = (DeferredMessagePipeline)_bus.Configuration.PipelineFactory.GetPipeline<DeferredMessagePipeline>(_bus);

			pipeline.Execute(_checkpointMessageId, _nextDeferredProcessDate);

			_nextDeferredProcessDate = pipeline.State.Get<DateTime>(StateKeys.NextDeferredProcessDate);

			if (_checkpointMessageId != pipeline.State.Get<Guid>(StateKeys.CheckpointMessageId))
			{
				_checkpointMessageId = pipeline.State.Get<Guid>(StateKeys.CheckpointMessageId);

				return;
			}

			_checkpointMessageId = Guid.Empty;
			_bus.Configuration.Inbox.ResetDeferredProcessing(_nextDeferredProcessDate);
		}
	}
}

