using System;

namespace Shuttle.Scheduling
{
	public interface IScheduleFactory
	{
		Schedule Build(string name, string inboxWorkQueueUri, string cronExpression);
		Schedule Build(string name, string inboxWorkQueueUri, string cronExpression, DateTime nextNotification);
	}
}