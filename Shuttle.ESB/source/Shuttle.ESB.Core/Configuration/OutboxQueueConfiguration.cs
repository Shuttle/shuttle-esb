using System;

namespace Shuttle.ESB.Core
{
    public class OutboxQueueConfiguration : IOutboxQueueConfiguration
    {
		private int threadCount;
		
		public OutboxQueueConfiguration()
        {
			ThreadCount = 1;
			MaximumFailureCount = 5;

            DurationToSleepWhenIdle = new[]
                                            {
                                                TimeSpan.FromMilliseconds(250), 
                                                TimeSpan.FromMilliseconds(500), 
                                                TimeSpan.FromSeconds(1),
                                                TimeSpan.FromSeconds(5)
                                            };

            DurationToIgnoreOnFailure = new[]
                                            {
                                                TimeSpan.FromMinutes(5), 
                                                TimeSpan.FromMinutes(30), 
                                                TimeSpan.FromHours(1)
                                            };
        }

        public IQueue WorkQueue { get; set; }
        public IQueue ErrorQueue { get; set; }

		public int ThreadCount
		{
			get { return threadCount; }
			set
			{
				threadCount = value > 0
								  ? value
								  : 5;
			}
		}

		public int MaximumFailureCount { get; set; }
        public TimeSpan[] DurationToIgnoreOnFailure { get; set; }
        public TimeSpan[] DurationToSleepWhenIdle { get; set; }
    }
}