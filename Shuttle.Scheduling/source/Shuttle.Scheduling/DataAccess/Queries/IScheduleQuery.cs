using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Scheduling
{
	public interface IScheduleQuery
	{
		DataTable All(DataSource source);
		bool HasScheduleStructures(DataSource source);
	}
}