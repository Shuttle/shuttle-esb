using Shuttle.Core.Data;

namespace Shuttle.Scheduling
{
    public class SchedulerData
    {
		public static readonly DataSource Source = new DataSource("Scheduler",
																  new SqlServerDbDataParameterFactory(),
																  new SqlServerContainsQueryFactory(),
																  new SqlServerInsertQueryFactory(),
																  new SqlServerUpdateQueryFactory(),
																  new SqlServerDeleteQueryFactory(),
																  new SqlServerSelectQueryFactory());
	}
}