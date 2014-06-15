using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Modules
{
	internal class ActiveTimeRangeObserver : IPipelineObserver<OnPipelineStarting>
	{
		private readonly IThreadState state;

		private readonly ActiveTimeRange range = new ActiveTimeRangeConfiguration().CreateActiveTimeRange();

		public ActiveTimeRangeObserver(IThreadState state)
		{
			Guard.AgainstNull(state, "state");

			this.state = state;
		}

		public void Execute(OnPipelineStarting pipelineEvent)
		{
			const int SLEEP = 15000;

			if (range.Active())
			{
				return;
			}

			pipelineEvent.Pipeline.Abort();

			ThreadSleep.While(SLEEP, state);
		}
	}
}