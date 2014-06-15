using System;
using System.Globalization;
using System.Xml.Serialization;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Modules
{
	[XmlType("ActiveTimeRange")]
	public class ActiveTimeRange
	{
		private bool wholeDay;

		public ActiveTimeRange()
		{
			WholeDay();
		}

		private void WholeDay()
		{
			wholeDay = true;

			activeFromHour = 0;
			activeFromMinute = 0;
			activeToHour = 23;
			activeToMinute = 59;
		}

		private string activeFromTime;
		private int activeFromHour;
		private int activeFromMinute;

		[XmlAttribute("activeFromTime")]
		public string ActiveFromTime
		{
			get { return activeFromTime; }
			set
			{
				if (value == "*")
				{
					WholeDay();

					return;
				}

				wholeDay = false;

				activeFromTime = value;

				DateTime dt;

				if (!DateTime.TryParseExact(value, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
				{
					throw new ArgumentException(string.Format(ESBModuleResources.InvalidActiveFromTime, value));
				}
				else
				{
					activeFromHour = dt.Hour;
					activeFromMinute = dt.Minute;
				}
			}
		}

		private string activeToTime;
		private int activeToHour;
		private int activeToMinute;

		[XmlAttribute("activeToTime")]
		public string ActiveToTime
		{
			get { return activeToTime; }
			set
			{
				if (value == "*")
				{
					WholeDay();

					return;
				}

				wholeDay = false;

				activeToTime = value;

				DateTime dt;

				if (!DateTime.TryParseExact(value, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
				{
					throw new ArgumentException(string.Format(ESBModuleResources.InvalidActiveToTime, value));
				}
				else
				{
					activeToHour = dt.Hour;
					activeToMinute = dt.Minute;
				}
			}
		}

		public bool Active()
		{
			return Active(DateTime.Now);
		}

		public bool Active(DateTime date)
		{
			return
				wholeDay
				||
				(
					date >= date.Date.AddHours(activeFromHour).AddMinutes(activeFromMinute)
					&&
					date <= date.Date.AddHours(activeToHour).AddMinutes(activeToMinute)
				);
		}
	}
}