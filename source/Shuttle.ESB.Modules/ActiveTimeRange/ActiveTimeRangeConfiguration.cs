using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Modules
{
	public class ActiveTimeRangeConfiguration : IActiveTimeRangeConfiguration
	{
		private readonly ConfigurationItem<string> activeFromTime;
		private readonly ConfigurationItem<string> activeToTime;

		public ActiveTimeRangeConfiguration()
		{
			activeFromTime = ConfigurationItem<string>.ReadSetting("ActiveFromTime", "*");
			activeToTime = ConfigurationItem<string>.ReadSetting("ActiveToTime", "*");
		}

		public string ActiveFromTime
		{
			get { return activeFromTime.GetValue(); }
		}

		public string ActiveToTime
		{
			get { return activeToTime.GetValue(); }
		}

		public ActiveTimeRange CreateActiveTimeRange()
		{
			return new ActiveTimeRange
				{
					ActiveFromTime = ActiveFromTime,
					ActiveToTime = ActiveToTime
				};
		}
	}
}