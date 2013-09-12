using System;
using Shuttle.Core.Domain;

namespace Shuttle.Scheduling
{
	public class SendNotification : IDomainEvent
	{
		public SendNotification(Schedule schedule, DateTime due)
		{
			this.Schedule = schedule;
			Due = due;
		}

		public Schedule Schedule { get; private set; }
		public DateTime Due { get; private set; }
	}
}