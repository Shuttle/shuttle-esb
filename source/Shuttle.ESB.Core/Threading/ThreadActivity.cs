using System;
using System.Threading;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class ThreadActivity : IThreadActivity
    {
        private static readonly TimeSpan defaultDuration = TimeSpan.FromMilliseconds(250);

        private int durationIndex;
        private readonly TimeSpan[] durations;

        public ThreadActivity(TimeSpan[] durationToSleepWhenIdle)
        {
            Guard.AgainstNull(durationToSleepWhenIdle, "durationToSleepWhenIdle");

            durations = durationToSleepWhenIdle;
            durationIndex = 0;
        }

        public ThreadActivity(IThreadActivityConfiguration threadActivityConfiguration)
        {
            Guard.AgainstNull(threadActivityConfiguration, "threadActivityConfiguration");

            durations = threadActivityConfiguration.DurationToSleepWhenIdle;
            durationIndex = 0;
        }

        private TimeSpan GetSleepTimeSpan()
        {
            if (durations == null || durations.Length == 0)
            {
                return defaultDuration;
            }

            if (durationIndex >= durations.Length)
            {
                durationIndex = durations.Length - 1;
            }

            return durations[durationIndex++];
        }

        public void Waiting(IThreadState state)
        {
        	var ms = (int) GetSleepTimeSpan().TotalMilliseconds;

        	ThreadSleep.While(ms, state);
        }

        public void Working()
        {
            durationIndex = 0;
        }
    }
}