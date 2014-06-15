namespace Shuttle.ESB.Modules
{
	public interface IActiveTimeRangeConfiguration
	{
		string ActiveFromTime { get; }
		string ActiveToTime { get; }

		ActiveTimeRange CreateActiveTimeRange();
	}
}