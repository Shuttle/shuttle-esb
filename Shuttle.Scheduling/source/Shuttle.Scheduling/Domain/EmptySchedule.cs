using System;

namespace Shuttle.Scheduling
{
	public class EmptySchedule : Schedule
	{
		public static Schedule Instance = new EmptySchedule();

		internal EmptySchedule()
			: base(string.Empty, string.Empty, null, DateTime.MinValue)
		{
		}

		protected override bool ShouldSendNotification
		{
			get { return false; }
		}
	}
}