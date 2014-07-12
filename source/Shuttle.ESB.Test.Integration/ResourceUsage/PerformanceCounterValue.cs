using System;
using System.Diagnostics;

namespace Shuttle.ESB.Test.Integration
{
	public class PerformanceCounterValue
	{
		private PerformanceCounter _counter;
		private float _lastReadValue;
		private DateTime _lastReadAt = DateTime.MinValue;

		public PerformanceCounterValue(PerformanceCounter counter)
		{
			_counter = counter;
		}

		public float NextValue()
		{
			if ((DateTime.Now - _lastReadAt).TotalMilliseconds >= 1000)
			{
				_lastReadValue = _counter.NextValue();
				_lastReadAt = DateTime.Now;
			}

			return _lastReadValue;
		}
	}
}