using System;
using Shuttle.Core.Data;

namespace Shuttle.Scheduling
{
	public class ScheduleTableAccess
	{
		public const string TableName = "Schedule";

		public static IQuery All()
		{
			return SelectBuilder
				.Select(ScheduleColumns.Name)
				.With(ScheduleColumns.InboxWorkQueueUri)
				.With(ScheduleColumns.CronExpression)
				.With(ScheduleColumns.NextNotification)
				.From(TableName);
		}

		public static IQuery Add(string name, string inboxWorkQueueUri, string cronExpression, DateTime nextNotification)
		{
			return InsertBuilder.Insert()
				.Add(ScheduleColumns.Name).WithValue(name)
				.Add(ScheduleColumns.InboxWorkQueueUri).WithValue(inboxWorkQueueUri)
				.Add(ScheduleColumns.CronExpression).WithValue(cronExpression)
				.Add(ScheduleColumns.NextNotification).WithValue(nextNotification)
				.Into(TableName);
		}

		public static IQuery Remove(string name)
		{
			return DeleteBuilder.Where(ScheduleColumns.Name).EqualTo(name).From(TableName);
		}

		public static IQuery Contains(string name)
		{
			return ContainsBuilder.Where(ScheduleColumns.Name).EqualTo(name).In(TableName);
		}

		public static IQuery Save(string name, string inboxWorkQueueUri, string cronExpression, DateTime nextNotification)
		{
			return UpdateBuilder.Update()
				.Set(ScheduleColumns.InboxWorkQueueUri).ToValue(inboxWorkQueueUri)
				.Set(ScheduleColumns.CronExpression).ToValue(cronExpression)
				.Set(ScheduleColumns.NextNotification).ToValue(nextNotification)
				.Where(ScheduleColumns.Name).EqualTo(name)
                .In(TableName);
		}

		public static IQuery SaveNextNotification(Schedule schedule)
		{
			return UpdateBuilder.Update()
				.Set(ScheduleColumns.NextNotification).ToValue(schedule.NextNotification)
				.Where(ScheduleColumns.Name).EqualTo(schedule.Id)
				.In(TableName);
		}
	}
}