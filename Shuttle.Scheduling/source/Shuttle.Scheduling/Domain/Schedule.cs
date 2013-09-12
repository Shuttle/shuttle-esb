using System;
using Shuttle.Core.Domain;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Scheduling
{
    public class Schedule : RowIdentity<string>
    {
        internal Schedule(string id, string inboxWOrkQueueUri, CronExpression cronExpression, DateTime nextNotification)
            : base(id)
        {
            InboxWorkQueueUri = inboxWOrkQueueUri;
            CronExpression = cronExpression;
            NextNotification = nextNotification;
        }

        public string InboxWorkQueueUri { get; private set; }
        public CronExpression CronExpression { get; private set; }
        public DateTime NextNotification { get; private set; }

        protected virtual bool ShouldSendNotification
        {
            get { return DateTime.Now >= NextNotification; }
        }

        public void SetNextNotification()
        {
            NextNotification = CronExpression.NextOccurrence();
        }

        public void Apply(string queue, CronExpression expression)
        {
            InboxWorkQueueUri = queue;
            CronExpression = expression;

            SetNextNotification();
        }

        public void CheckNotification()
        {
            if (!ShouldSendNotification)
            {
                return;
            }

            var due = NextNotification;

            SetNextNotification();

            DomainEvents.Raise(new SendNotification(this, due));
        }
    }
}