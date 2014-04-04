using System;

namespace Shuttle.ESB.Core
{
    public class InboxQueueConfiguration : IInboxQueueConfiguration
    {
		private bool messageDeferred;
		private readonly object messageDeferredLock = new object();
		private DateTime _nextDeferredProcessDate = DateTime.MinValue;
        private int _threadCount;

        public InboxQueueConfiguration()
        {
            ThreadCount = 5;
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

        public QueueStartupAction WorkQueueStartupAction { get; set; }
        public IQueue WorkQueue { get; set; }
        public IQueue ErrorQueue { get; set; }
        public bool Distribute { get; set; }
	    public IQueue DeferredQueue { get; set; }

	    public void ResetDeferredProcessing(DateTime nextDeferredProcessDate)
	    {
		    lock (messageDeferredLock)
		    {
			    if (messageDeferred)
			    {
				    messageDeferred = false;
				    return;
			    }

			    _nextDeferredProcessDate = nextDeferredProcessDate;
		    }
	    }

	    public void MessageDeferred(DateTime ignoreTillDate)
	    {
		    lock (messageDeferredLock)
		    {
			    messageDeferred = true;

			    if (_nextDeferredProcessDate > ignoreTillDate)
			    {
				    _nextDeferredProcessDate = ignoreTillDate;
			    }
		    }
	    }

	    public bool ShouldProcessDeferred()
	    {
		    return (DateTime.Now >= _nextDeferredProcessDate);
	    }

	    public int ThreadCount
        {
            get { return _threadCount; }
            set
            {
                _threadCount = value > 0
                                  ? value
                                  : 5;
            }
        }

        public int MaximumFailureCount { get; set; }
        public TimeSpan[] DurationToIgnoreOnFailure { get; set; }
        public TimeSpan[] DurationToSleepWhenIdle { get; set; }
    }
}