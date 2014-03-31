using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public abstract class MessagePipeline : ObservablePipeline
	{
		protected readonly IServiceBus _bus;

	    protected MessagePipeline(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			_bus = bus;

			State.Add(bus);
		}

	    protected MessagePipeline()
		{
		}

		public void SetWorkQueue(IQueue queue)
		{
			State.Add(StateKeys.WorkQueue, queue);
		}

		public void SetDeferredQueue(IQueue queue)
		{
			State.Add(StateKeys.DeferredQueue, queue);
		}

		public void SetCheckpointMessageId(Guid checkpointMessageId)
		{
			State.Add(StateKeys.CheckpointMessageId, checkpointMessageId);
		}

		public void SetErrorQueue(IQueue queue)
		{
			State.Add(StateKeys.ErrorQueue, queue);
		}

		public void SetDestinationQueue(IQueue queue)
		{
			State.Add(StateKeys.DestinationQueue, queue);
		}

		public void SetIgnoreTillDate(DateTime date)
		{
			State.Add(StateKeys.IgnoreTillDate, date);
		}

		public void SetMessage(object message)
		{
			State.Add(StateKeys.Message, message);
		}

		public void SetTransportMessage(TransportMessage transportMessage)
		{
			State.Add(StateKeys.TransportMessage, transportMessage);
		}

		public void SetMaximumFailureCount(int count)
		{
			State.Add(StateKeys.MaximumFailureCount, count);
		}

		public void SetDurationToIgnoreOnFailure(TimeSpan[] timeSpans)
		{
			State.Add(StateKeys.DurationToIgnoreOnFailure, timeSpans);
		}

		public virtual void Obtained()
		{
			State.Clear();
			State.Add(_bus);

			_bus.Events.OnPipelineObtained(this, new PipelineEventArgs(this));
		}

		public void Released()
		{
			_bus.Events.OnPipelineReleased(this, new PipelineEventArgs(this));
		}
	}
}