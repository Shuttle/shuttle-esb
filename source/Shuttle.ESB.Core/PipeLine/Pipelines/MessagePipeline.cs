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

		public IQueue WorkQueue
		{
			get { return State.Get<IQueue>(StateKeys.WorkQueue); }
			set { State.Add(StateKeys.WorkQueue, value); }
		}

		public IQueue JournalQueue
		{
			get { return State.Get<IQueue>(StateKeys.JournalQueue); }
			set { State.Add(StateKeys.JournalQueue, value); }
		}

		public IQueue ErrorQueue
		{
			get { return State.Get<IQueue>(StateKeys.ErrorQueue); }
			set { State.Add(StateKeys.ErrorQueue, value); }
		}

		public IQueue DestinationQueue
		{
			get { return State.Get<IQueue>(StateKeys.DestinationQueue); }
			set { State.Add(StateKeys.DestinationQueue, value); }
		}

		public DateTime IgnoreTillDate
		{
			get { return State.Get<DateTime>(StateKeys.IgnoreTillDate); }
			set { State.Add(StateKeys.IgnoreTillDate, value); }
		}

		public object Message
		{
			get { return State.Get<object>(StateKeys.Message); }
			set { State.Add(StateKeys.Message, value); }
		}

		public TransportMessage TransportMessage
		{
			get { return State.Get<TransportMessage>(StateKeys.TransportMessage); }
			set { State.Add(StateKeys.TransportMessage, value); }
		}

		public int MaximumFailureCount
		{
			get { return State.Get<int>(StateKeys.MaximumFailureCount); }
			set { State.Add(StateKeys.MaximumFailureCount, value); }
		}

		public TimeSpan[] DurationToIgnoreOnFailure
		{
			get { return State.Get<TimeSpan[]>(StateKeys.DurationToIgnoreOnFailure); }
			set { State.Add(StateKeys.DurationToIgnoreOnFailure, value); }
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