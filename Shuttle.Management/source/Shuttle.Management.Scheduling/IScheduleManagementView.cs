using System;
using System.Collections.Generic;
using Shuttle.Management.Shell;

namespace Shuttle.Management.Scheduling
{
	public interface IScheduleManagementView
	{
		void AddSchedule(string name, string inboxWorkQueueUri, string cronExpression, DateTime nextNotification);
		List<string> GetSelectedScheduleNames();
		void MarkAllSchedules();
		void InvertMarkedSchedules();
		void ClearSchedules();
		void PopulateDataStores(IEnumerable<DataStore> list);
		string DataStoreValue { get; }
		void CheckAllSchedules();
		void InvertScheduleChecks();
		string ScheduleNameValue { get; set; }
		string EndpointInboxWorkQueueUriValue { get; set; }
		string CronExpressionValue { get; set; }
		IEnumerable<string> SelectedScheduleNames { get; }
		void PopulateInboxWorkQueueUris(IEnumerable<Queue> list);
	}
}