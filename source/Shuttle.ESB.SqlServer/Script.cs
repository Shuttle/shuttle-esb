using System.Configuration;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.SqlServer
{
    public class Script
    {
        public static readonly Script QueueCount = new Script("QueueCount");
        public static readonly Script QueueCreate = new Script("QueueCreate");
        public static readonly Script QueueDequeue = new Script("QueueDequeue");
        public static readonly Script QueueDequeueId = new Script("QueueDequeueId");
        public static readonly Script QueueDrop = new Script("QueueDrop");
        public static readonly Script QueueEnqueue = new Script("QueueEnqueue");
        public static readonly Script QueueExists = new Script("QueueExists");
        public static readonly Script QueuePurge = new Script("QueuePurge");
        public static readonly Script QueueRemove = new Script("QueueRemove");
        public static readonly Script QueueRead = new Script("QueueRead");

        public static readonly Script SubscriptionManagerInboxWorkQueueUris = new Script("SubscriptionManagerInboxWorkQueueUris");
        public static readonly Script SubscriptionManagerExists = new Script("SubscriptionManagerExists");
        public static readonly Script SubscriptionManagerSubscribe = new Script("SubscriptionManagerSubscribe");

        public static readonly Script IdempotenceTrackerExists = new Script("IdempotenceTrackerExists");
        public static readonly Script IdempotenceTrackerAdd = new Script("IdempotenceTrackerAdd");
        public static readonly Script IdempotenceTrackerRemove = new Script("IdempotenceTrackerRemove");
        public static readonly Script IdempotenceTrackerContains = new Script("IdempotenceTrackerContains");

		public static readonly Script DeferredMessageManagerExists = new Script("DeferredMessageManagerExists");
		public static readonly Script DeferredMessageManagerRegister = new Script("DeferredMessageManagerRegister");

        private Script(string name)
        {
            Name = name;

            var key = string.Format("{0}FileName", name);

            var value = ConfigurationManager.AppSettings[key];

            if (!string.IsNullOrEmpty(value))
            {
                FileName = value;

                return;
            }

            FileName = string.Format("{0}.sql", name);

            Log.For(this).Information(string.Format("The application configuration AppSettings section does not contain a key '{0}'.  Using default value of '{1}'.", key, FileName));
        }

        public string Name { get; private set; }
        public string FileName { get; private set; }
    }
}