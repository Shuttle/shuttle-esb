using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Scheduling
{
	public class ScheduleFactory : IScheduleFactory
	{
		public Schedule Build(string name, string inboxWorkQueueUri, string cronExpression)
		{
			var expression = new CronExpression(cronExpression);

			return Build(name, inboxWorkQueueUri, expression, expression.NextOccurrence(DateTime.Now));
		}

		private static Schedule Build(string name, string inboxWorkQueueUri, CronExpression cronExpression, DateTime nextNotification)
		{
			return new Schedule(name, inboxWorkQueueUri, cronExpression, nextNotification);
		}

		public Schedule Build(string name, string inboxWorkQueueUri, string cronExpression, DateTime nextNotification)
		{
			return new Schedule(name, inboxWorkQueueUri, new CronExpression(cronExpression), nextNotification);
		}
	}
}