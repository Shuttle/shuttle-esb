using System;

namespace Shuttle.ESB.Core
{
    public class WorkerStartedEvent
    {
        public string InboxWorkQueueUri { get; set; }
        public DateTime DateStarted { get; set; }
    }
}