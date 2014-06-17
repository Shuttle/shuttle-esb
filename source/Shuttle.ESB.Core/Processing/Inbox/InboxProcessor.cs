using System.Diagnostics;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class InboxProcessor : IProcessor
	{
		protected readonly IServiceBus _bus;
		protected readonly IThreadActivity _threadActivity;

		public InboxProcessor(IServiceBus bus, IThreadActivity threadActivity)
		{
			Guard.AgainstNull(bus, "bus");
			Guard.AgainstNull(threadActivity, "threadActivity");

			_bus = bus;
			_threadActivity = threadActivity;
		}

		[DebuggerNonUserCode]
		void IProcessor.Execute(IThreadState state)
		{
			Execute(state);
		}

		public virtual void Execute(IThreadState state)
		{
			var availableWorker = _bus.Configuration.WorkerAvailabilityManager.GetAvailableWorker();

			if (_bus.Configuration.Inbox.Distribute && availableWorker == null)
			{
				_threadActivity.Waiting(state);

				return;
			}

			var messagePipeline = availableWorker == null
				                      ? _bus.Configuration.PipelineFactory.GetPipeline<InboxMessagePipeline>(_bus)
				                      : (MessagePipeline) _bus.Configuration.PipelineFactory.GetPipeline<DistributorPipeline>(_bus);

			try
			{
				messagePipeline.State.SetAvailableWorker(availableWorker);
				messagePipeline.State.ResetWorking();
				messagePipeline.State.SetActiveState(state);

				if (!state.Active)
				{
					return;
				}

				messagePipeline.Execute();

				if (messagePipeline.State.Get<bool>(StateKeys.Working))
				{
					_bus.Events.OnThreadWorking(this, new ThreadStateEventArgs(typeof (InboxMessagePipeline)));

					_threadActivity.Working();
				}
				else
				{
					_bus.Events.OnThreadWaiting(this, new ThreadStateEventArgs(typeof (InboxMessagePipeline)));

					_threadActivity.Waiting(state);
				}
			}
			finally
			{
				_bus.Configuration.PipelineFactory.ReleasePipeline(messagePipeline);
			}
		}
	}
}