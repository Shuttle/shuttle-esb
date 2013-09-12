using System.Diagnostics;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class InboxProcessor : IProcessor
    {
        protected readonly IServiceBus bus;
        protected readonly IThreadActivity threadActivity;

        public InboxProcessor(IServiceBus bus, IThreadActivity threadActivity)
        {
            Guard.AgainstNull(bus, "bus");
            Guard.AgainstNull(threadActivity, "threadActivity");

            this.bus = bus;
            this.threadActivity = threadActivity;
        }

        [DebuggerNonUserCode]
        void IProcessor.Execute(IActiveState state)
        {
            Execute(state);
        }

        public virtual void Execute(IActiveState state)
        {
            var availableWorker = bus.Configuration.WorkerAvailabilityManager.GetAvailableWorker();

            if (bus.Configuration.Inbox.Distribute && availableWorker == null)
            {
                threadActivity.Waiting(state);

                return;
            }

            var messagePipeline = availableWorker == null
                                      ? bus.Configuration.PipelineFactory.GetPipeline<InboxMessagePipeline>(bus)
                                      : bus.Configuration.PipelineFactory.GetPipeline<DistributorPipeline>(bus);

            try
            {
                messagePipeline.State.Replace(StateKeys.AvailableWorker, availableWorker);
                messagePipeline.State.Replace(StateKeys.Working, false);
                messagePipeline.State.Replace(StateKeys.ActiveState, state);

                messagePipeline.Execute();

                if (messagePipeline.State.Get<bool>(StateKeys.Working))
                {
                    bus.Events.OnThreadWorking(this, new ThreadStateEventArgs(typeof(InboxMessagePipeline)));

                    threadActivity.Working();
                }
                else
                {
                    bus.Events.OnThreadWaiting(this, new ThreadStateEventArgs(typeof(InboxMessagePipeline)));

                    threadActivity.Waiting(state);
                }
            }
            finally
            {
                bus.Configuration.PipelineFactory.ReleasePipeline(messagePipeline);
            }
        }
    }
}