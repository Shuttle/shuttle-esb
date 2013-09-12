using System;

namespace Shuttle.ESB.Core
{
    public class WorkerThreadAvailableCommand
    {
        public Guid Identifier { get; set; }
        public string InboxWorkQueueUri { get; set; }
        public DateTime DateSent { get; set; }
    }
}