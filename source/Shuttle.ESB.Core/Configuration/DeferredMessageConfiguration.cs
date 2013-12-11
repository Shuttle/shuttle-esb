using System;

namespace Shuttle.ESB.Core
{
	public class DeferredMessageConfiguration : IDeferredMessageConfiguration
	{
		public TimeSpan[] DurationToSleepWhenIdle { get; set; }
		public IDeferredMessageQueue DeferredMessageQueue { get; set; }
	}
}