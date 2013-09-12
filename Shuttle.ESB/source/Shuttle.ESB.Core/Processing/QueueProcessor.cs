using System.Diagnostics;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public abstract class QueueProcessor<TMessagePipeline> : IProcessor
        where TMessagePipeline : MessagePipeline
    {
        protected readonly IServiceBus bus;
        protected readonly IThreadActivity threadActivity;

        protected QueueProcessor(IServiceBus bus, IThreadActivity threadActivity)
        {
            Guard.AgainstNull(bus, "bus");
            Guard.AgainstNull(threadActivity, "threadActivity");

            this.bus = bus;
            this.threadActivity = threadActivity;
        }

        public virtual void Execute(IActiveState state)
        {
            var messagePipeline = bus.Configuration.PipelineFactory.GetPipeline<TMessagePipeline>(bus);

            try
            {
                messagePipeline.State.Replace(StateKeys.Working, false);
                messagePipeline.State.Replace(StateKeys.ActiveState, state);

                messagePipeline.Execute();

                if (messagePipeline.State.Get<bool>(StateKeys.Working))
                {
                    bus.Events.OnThreadWorking(this, new ThreadStateEventArgs(typeof(TMessagePipeline)));

                    threadActivity.Working();
                }
                else
                {
                    bus.Events.OnThreadWaiting(this, new ThreadStateEventArgs(typeof(TMessagePipeline)));

                    threadActivity.Waiting(state);
                }
            }
            finally
            {
                bus.Configuration.PipelineFactory.ReleasePipeline(messagePipeline);
            }
        }

        [DebuggerNonUserCode]
        void IProcessor.Execute(IActiveState state)
        {
            Execute(state);
        }
    }
}