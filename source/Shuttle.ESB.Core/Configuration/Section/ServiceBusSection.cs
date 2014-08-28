using System.Configuration;
using System.IO;

namespace Shuttle.ESB.Core
{
    public class ServiceBusSection : ConfigurationSection
    {
        public static ServiceBusSection Open(string file)
        {
			return ShuttleConfigurationSection.Open<ServiceBusSection>("serviceBus", file);
        }

        [ConfigurationProperty("messageRoutes", IsRequired = false, DefaultValue = null)]
        public MessageRouteElementCollection MessageRoutes
        {
			get { return (MessageRouteElementCollection)this["messageRoutes"]; }
        }

		[ConfigurationProperty("forwardingRoutes", IsRequired = false, DefaultValue = null)]
		public MessageRouteElementCollection ForwardingRoutes
        {
			get { return (MessageRouteElementCollection)this["forwardingRoutes"]; }
        }

		[ConfigurationProperty("queueFactories", IsRequired = false, DefaultValue = null)]
		public QueueFactoriesElement QueueFactories
        {
			get { return (QueueFactoriesElement)this["queueFactories"]; }
        }

        [ConfigurationProperty("inbox", IsRequired = false, DefaultValue = null)]
        public InboxElement Inbox
        {
            get { return (InboxElement) this["inbox"]; }
        }

        [ConfigurationProperty("control", IsRequired = false, DefaultValue = null)]
        public ControlInboxElement ControlInbox
        {
			get { return (ControlInboxElement)this["control"]; }
        }

        [ConfigurationProperty("outbox", IsRequired = false, DefaultValue = null)]
        public OutboxElement Outbox
        {
			get { return (OutboxElement)this["outbox"]; }
        }

        [ConfigurationProperty("createQueues", IsRequired = false, DefaultValue = true)]
		public bool CreateQueues
        {
            get { return (bool) this["createQueues"]; }
        }

        [ConfigurationProperty("worker", IsRequired = false)]
        public WorkerElement Worker
        {
            get { return (WorkerElement) this["worker"]; }
        }

        [ConfigurationProperty("transactionScope", IsRequired = false, DefaultValue = null)]
        public TransactionScopeElement TransactionScope
        {
            get { return (TransactionScopeElement)this["transactionScope"]; }
        }

        [ConfigurationProperty("removeMessagesNotHandled", IsRequired = false, DefaultValue = true)]
        public bool RemoveMessagesNotHandled
        {
            get { return (bool)this["removeMessagesNotHandled"]; }
        }

        [ConfigurationProperty("encryptionAlgorithm", IsRequired = false, DefaultValue = "")]
        public string EncryptionAlgorithm
        {
            get { return (string)this["encryptionAlgorithm"]; }
        }

        [ConfigurationProperty("compressionAlgorithm", IsRequired = false, DefaultValue = "")]
        public string CompressionAlgorithm
        {
            get { return (string)this["compressionAlgorithm"]; }
        }
    }
}