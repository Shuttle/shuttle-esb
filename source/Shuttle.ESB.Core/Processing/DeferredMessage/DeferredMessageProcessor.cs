using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DeferredMessageProcessor : IProcessor
	{
		private bool _messageDeferred;
		private readonly object _messageDeferredLock = new object();
		private DateTime _nextDeferredProcessDate = DateTime.MinValue;
		private Guid _checkpointMessageId = Guid.Empty;

		private readonly IServiceBus _bus;

		public DeferredMessageProcessor(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			_bus = bus;
		}

		public void Execute(IActiveState state)
		{
			if (!ShouldProcessDeferred())
			{
				ThreadSleep.While(1000, state);

				return;
			}

			lock (_messageDeferredLock)
			{
				_messageDeferred = false;
			}

			var pipeline = (DeferredMessagePipeline)_bus.Configuration.PipelineFactory.GetPipeline<DeferredMessagePipeline>(_bus);

			pipeline.State.SetCheckpointMessageId(_checkpointMessageId);
			pipeline.State.SetNextDeferredProcessDate(_nextDeferredProcessDate);
			pipeline.State.SetDeferredMessageReturned(false);

			pipeline.Execute();

			var nextDeferredProcessDate = pipeline.State.Get<DateTime>(StateKeys.NextDeferredProcessDate);

			if (_messageDeferred)
			{
				if (nextDeferredProcessDate < _nextDeferredProcessDate)
				{
					_nextDeferredProcessDate = nextDeferredProcessDate;
				}
			}
			else
			{
				_nextDeferredProcessDate = nextDeferredProcessDate;
			}

			if (_checkpointMessageId != pipeline.State.Get<Guid>(StateKeys.CheckpointMessageId))
			{
				_checkpointMessageId = pipeline.State.Get<Guid>(StateKeys.CheckpointMessageId);

				return;
			}

			_checkpointMessageId = Guid.Empty;
		}

		public void MessageDeferred(DateTime ignoreTillDate)
		{
			lock (_messageDeferredLock)
			{
				_messageDeferred = true;

				if (_nextDeferredProcessDate > ignoreTillDate)
				{
					_nextDeferredProcessDate = ignoreTillDate;
				}
			}
		}

		private bool ShouldProcessDeferred()
		{
			return (DateTime.Now >= _nextDeferredProcessDate);
		}

	}
}

