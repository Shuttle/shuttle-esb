using System;

namespace Shuttle.Scheduling
{
	public class RunScheduleCommand
	{
		public string Name { get; set; }
		public string ServerName { get; set; }
		public DateTime DateDue { get; set; }
		public DateTime DateSent { get; set; }
	}
}