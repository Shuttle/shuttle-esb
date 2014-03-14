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

        public static readonly Script IdempotenceServiceExists = new Script("IdempotenceServiceExists");
        public static readonly Script IdempotenceInitialize = new Script("IdempotenceInitialize");
        public static readonly Script IdempotenceProcessing = new Script("IdempotenceProcessing");
        public static readonly Script IdempotenceComplete = new Script("IdempotenceComplete");
        public static readonly Script IdempotenceIsProcessing = new Script("IdempotenceIsProcessing");
        public static readonly Script IdempotenceHasCompleted = new Script("IdempotenceHasCompleted");
        public static readonly Script IdempotenceSendDeferredMessage = new Script("IdempotenceSendDeferredMessage");
		public static readonly Script IdempotenceDeferredMessageSent = new Script("IdempotenceDeferredMessageSent");
		public static readonly Script IdempotenceGetDeferredMessages = new Script("IdempotenceGetDeferredMessages");

		public static readonly Script DeferredMessageExists = new Script("DeferredMessageExists");
		public static readonly Script DeferredMessageEnqueue = new Script("DeferredMessageEnqueue");
		public static readonly Script DeferredMessageDequeue = new Script("DeferredMessageDequeue");
		public static readonly Script DeferredMessagePurge = new Script("DeferredMessagePurge");
		public static readonly Script DeferredMessageCount = new Script("DeferredMessageCount");

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