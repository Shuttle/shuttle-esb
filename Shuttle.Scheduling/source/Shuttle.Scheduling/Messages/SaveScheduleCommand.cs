namespace Shuttle.Scheduling
{
	public class SaveScheduleCommand
	{
		public string Name { get; set; }
		public string CronExpression { get; set; }
		public string InboxWorkQueueUri { get; set; }
	}
}