using Shuttle.Core.Data;

namespace Shuttle.Scheduling
{
	public class ScheduleQueries
	{
		public const string TableName = "Schedule";

		public static IQuery All()
		{
			return SelectBuilder
				.Select(ScheduleColumns.Name)
				.With(ScheduleColumns.InboxWorkQueueUri)
				.With(ScheduleColumns.CronExpression)
				.With(ScheduleColumns.NextNotification)
				.OrderBy(ScheduleColumns.Name).Ascending()
				.From(TableName);
		}

		public static IQuery HasScheduleStructures()
		{
			return
				RawQuery.CreateFrom(
					"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Schedule') select 1 ELSE select 0");
		}		
	}
}