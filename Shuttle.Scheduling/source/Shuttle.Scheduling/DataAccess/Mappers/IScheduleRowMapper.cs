using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Scheduling
{
	public class IScheduleRowMapper : IDataRowMapper<Schedule>
	{
		private readonly IScheduleFactory scheduleFactory;

		public IScheduleRowMapper(IScheduleFactory scheduleFactory)
		{
			this.scheduleFactory = scheduleFactory;
		}

		public MappedRow<Schedule> Map(DataRow row)
		{
			return new MappedRow<Schedule>(row,
			                               scheduleFactory.Build(ScheduleColumns.Name.MapFrom(row),
			                                                     ScheduleColumns.InboxWorkQueueUri.MapFrom(row),
			                                                     ScheduleColumns.CronExpression.MapFrom(row),
			                                                     ScheduleColumns.NextNotification.MapFrom(row)));
		}
	}
}