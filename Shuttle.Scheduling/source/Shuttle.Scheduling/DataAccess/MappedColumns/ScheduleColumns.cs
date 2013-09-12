using System;
using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Scheduling
{
	public class ScheduleColumns
	{
		public static readonly MappedColumn<string> Name = new MappedColumn<string>("Name", DbType.AnsiString, 120);
		public static readonly MappedColumn<string> InboxWorkQueueUri = new MappedColumn<string>("InboxWorkQueueUri", DbType.AnsiString, 130);
		public static readonly MappedColumn<string> CronExpression = new MappedColumn<string>("CronExpression", DbType.AnsiString, 250);
		public static readonly MappedColumn<DateTime> NextNotification = new MappedColumn<DateTime>("NextNotification", DbType.DateTime);
	}
}