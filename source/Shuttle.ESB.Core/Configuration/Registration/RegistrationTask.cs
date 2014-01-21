using System;

namespace Shuttle.ESB.Core
{
	public abstract class RegistrationTask
	{
		protected readonly TimeSpan[] defaultDurationToIgnoreOnFailure =
			new[]
				{
					TimeSpan.FromMinutes(5),
					TimeSpan.FromMinutes(10),
					TimeSpan.FromMinutes(15),
					TimeSpan.FromMinutes(30),
					TimeSpan.FromMinutes(60)
				};

		protected readonly TimeSpan[] defaultDurationToSleepWhenIdle =
			(TimeSpan[])
			new StringDurationArrayConverter()
				.ConvertFrom("250ms*4,500ms*2,1s");

		public abstract void Execute(ServiceBusConfiguration configuration);

		protected TimeSpan[] DurationToSleepWhenIdle(TimeSpan[] durationToSleepWhenIdle)
		{
			return durationToSleepWhenIdle ?? defaultDurationToSleepWhenIdle;
		}

		protected TimeSpan[] DurationToIgnoreOnFailure(TimeSpan[] durationToIgnoreOnFailure)
		{
			return durationToIgnoreOnFailure ?? defaultDurationToIgnoreOnFailure;
		}
	}
}