using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Modules.ActiveTimeRange
{
	internal class ActiveTimeRangeObserver : IPipelineObserver<OnPipelineStarting>
	{
		private readonly IActiveState state;

		private readonly Shuttle.Core.Infrastructure.ActiveTimeRange range =
			new ActiveTimeRangeConfiguration().CreateActiveTimeRange();

		public ActiveTimeRangeObserver(IActiveState state)
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