using System.Diagnostics;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public abstract class QueueProcessor<TMessagePipeline> : IProcessor
        where TMessagePipeline : MessagePipeline
    {
        protected readonly IServiceBus _bus;
        protected readonly IThreadActivity _threadActivity;

        protected QueueProcessor(IServiceBus bus, IThreadActivity threadActivity)
        {
            Guard.AgainstNull(bus, "bus");
            Guard.AgainstNull(threadActivity, "threadActivity");

            _bus = bus;
            _threadActivity = threadActivity;
        }

        public virtual void Execute(IThreadState state)
        {
            var messagePipeline = _bus.Configuration.PipelineFactory.GetPipeline<TMessagePipeline>(_bus);

            try
            {
                messagePipeline.State.Replace(StateKeys.Working, false);
                messagePipeline.State.Replace(StateKeys.ActiveState, state);

                messagePipeline.Execute();

                if (messagePipeline.State.Get<bool>(StateKeys.Working))
                {
                    _bus.Events.OnThreadWorking(this, new ThreadStateEventArgs(typeof(TMessagePipeline)));

                    _threadActivity.Working();
                }
                else
                {
                    _bus.Events.OnThreadWaiting(this, new ThreadStateEventArgs(typeof(TMessagePipeline)));

                    _threadActivity.Waiting(state);
                }
            }
            finally
            {
                _bus.Configuration.PipelineFactory.ReleasePipeline(messagePipeline);
            }
        }

        [DebuggerNonUserCode]
        void IProcessor.Execute(IThreadState state)
        {
            Execute(state);
        }
    }
}