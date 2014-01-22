using System.Configuration;
using System.IO;

namespace Shuttle.ESB.Core
{
    public class ServiceBusSection : ConfigurationSection
    {
        public static ServiceBusSection Open(string file)
        {
            return ConfigurationManager
                       .OpenMappedMachineConfiguration(new ConfigurationFileMap(Path.GetFullPath(file)))
                       .GetSection("serviceBus") as ServiceBusSection;
        }

        private static readonly ConfigurationProperty removeMessagesNotHandled =
            new ConfigurationProperty("removeMessagesNotHandled", typeof (bool), true, ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty encryptionAlgorithm =
            new ConfigurationProperty("encryptionAlgorithm", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty compressionAlgorithm =
            new ConfigurationProperty("compressionAlgorithm", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty messageRoutes =
            new ConfigurationProperty("messageRoutes", typeof (MessageRouteElementCollection), null,
                                      ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty forwardingRoutes =
			new ConfigurationProperty("forwardingRoutes", typeof(MessageRouteElementCollection), null,
                                      ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty inbox =
            new ConfigurationProperty("inbox", typeof (InboxElement), null, ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty control =
            new ConfigurationProperty("control", typeof (ControlInboxElement), null, ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty outbox =
            new ConfigurationProperty("outbox", typeof (OutboxElement), null, ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty deferredMessage =
			new ConfigurationProperty("deferredMessage", typeof(DeferredMessageElement), null, ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty createQueues =
            new ConfigurationProperty("createQueues", typeof (bool), true, ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty worker =
            new ConfigurationProperty("worker", typeof (WorkerElement), null, ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty transactionScope =
            new ConfigurationProperty("transactionScope", typeof(TransactionScopeElement), new TransactionScopeElement(), ConfigurationPropertyOptions.None);

        public ServiceBusSection()
        {
            base.Properties.Add(removeMessagesNotHandled);
            base.Properties.Add(encryptionAlgorithm);
            base.Properties.Add(compressionAlgorithm);
            base.Properties.Add(messageRoutes);
            base.Properties.Add(inbox);
            base.Properties.Add(outbox);
            base.Properties.Add(control);
            base.Properties.Add(deferredMessage);
            base.Properties.Add(createQueues);
            base.Properties.Add(worker);
            base.Properties.Add(transactionScope);
        }

        [ConfigurationProperty("messageRoutes", IsRequired = false)]
        public MessageRouteElementCollection MessageRoutes
        {
            get { return (MessageRouteElementCollection) this[messageRoutes]; }
        }

		[ConfigurationProperty("forwardingRoutes", IsRequired = false)]
		public MessageRouteElementCollection ForwardingRoutes
        {
			get { return (MessageRouteElementCollection)this[forwardingRoutes]; }
        }

        [ConfigurationProperty("inbox", IsRequired = false)]
        public InboxElement Inbox
        {
            get { return (InboxElement) this[inbox]; }
        }

        [ConfigurationProperty("control", IsRequired = false)]
        public ControlInboxElement ControlInbox
        {
            get { return (ControlInboxElement) this[control]; }
        }

        [ConfigurationProperty("outbox", IsRequired = false)]
        public OutboxElement Outbox
        {
            get { return (OutboxElement) this[outbox]; }
        }

		[ConfigurationProperty("deferredMessage", IsRequired = false)]
		public DeferredMessageElement DeferredMessage
        {
			get { return (DeferredMessageElement)this[deferredMessage]; }
        }

        [ConfigurationProperty("createQueues", IsRequired = false, DefaultValue = true)]
		public bool CreateQueues
        {
            get { return (bool) this[createQueues]; }
        }

        [ConfigurationProperty("worker", IsRequired = false)]
        public WorkerElement Worker
        {
            get { return (WorkerElement) this[worker]; }
        }

        [ConfigurationProperty("transactionScope", IsRequired = false)]
        public TransactionScopeElement TransactionScope
        {
            get { return (TransactionScopeElement)this[transactionScope]; }
        }

        [ConfigurationProperty("removeMessagesNotHandled", IsRequired = false, DefaultValue = true)]
        public bool RemoveMessagesNotHandled
        {
            get { return (bool)this[removeMessagesNotHandled]; }
        }

        [ConfigurationProperty("encryptionAlgorithm", IsRequired = false, DefaultValue = "")]
        public string EncryptionAlgorithm
        {
            get { return (string)this[encryptionAlgorithm]; }
        }

        [ConfigurationProperty("compressionAlgorithm", IsRequired = false, DefaultValue = "")]
        public string CompressionAlgorithm
        {
            get { return (string)this[compressionAlgorithm]; }
        }
    }
}