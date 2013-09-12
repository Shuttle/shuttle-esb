using System.Collections.Generic;
using Shuttle.Core.Data;

namespace Shuttle.Scheduling
{
	public class ScheduleRepository : IScheduleRepository
	{
		private readonly IDatabaseGateway gateway;
		private readonly IDataRepository<Schedule> repository;

		public ScheduleRepository(IDataRepository<Schedule> repository, IDatabaseGateway gateway)
		{
			this.repository = repository;
			this.gateway = gateway;
		}

		public IEnumerable<Schedule> All()
		{
			return repository.FetchAllUsing(SchedulerData.Source, ScheduleTableAccess.All());
		}

		public void SaveNextNotification(Schedule schedule)
		{
            gateway.ExecuteUsing(SchedulerData.Source, ScheduleTableAccess.SaveNextNotification(schedule));
		}
	}
}