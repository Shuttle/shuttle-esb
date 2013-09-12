using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public enum QueueStartupAction
	{
		None = 0,
		Purge = 1
	}

	public enum QueueCreationType
	{
		None = 0,
		All = 1,
		Local = 2
	}

	public class ServiceBusConfiguration : IServiceBusConfiguration
	{
		public const string ServiceBusSectionName = "serviceBus";

		private static ServiceBusSection serviceBusSection;

		private readonly List<IEncryptionAlgorithm> encryptionAlgorithms = new List<IEncryptionAlgorithm>();
		private readonly List<ICompressionAlgorithm> compressionAlgorithms = new List<ICompressionAlgorithm>();

		private ISerializer serializer;
		private ISubscriptionManager subscriptionManager;

		public ServiceBusConfiguration()
		{
			WorkerAvailabilityManager = new WorkerAvailabilityManager();
			QueueManager = new QueueManager();
			Modules = new ModuleCollection();
		}

		public static ServiceBusSection ServiceBusSection
		{
			get
			{
				return serviceBusSection ?? (serviceBusSection = ConfigurationManager.GetSection(ServiceBusSectionName) as ServiceBusSection);
			}
		}

		public ISerializer Serializer
		{
			get { return serializer; }
			set
			{
				Guard.AgainstNull(value, "serializer");

				if (value.Equals(Serializer))
				{
					return;
				}

				var replay = Serializer as IReplay<ISerializer>;

				if (replay != null)
				{
					replay.Replay(value);
				}

				serializer = value;
			}
		}

		public IInboxQueueConfiguration Inbox { get; set; }
		public IControlInboxQueueConfiguration ControlInbox { get; set; }
		public IOutboxQueueConfiguration Outbox { get; set; }
		public IWorkerConfiguration Worker { get; set; }

        public IQueueManager QueueManager { get; set; }
        public IIdempotenceTracker IdempotenceTracker { get; set; }

		public ModuleCollection Modules { get; private set; }

		public IMessageHandlerFactory MessageHandlerFactory { get; set; }
		public IMessageRouteProvider MessageRouteProvider { get; set; }
		public IMessageRouteProvider ForwardingRouteProvider { get; set; }
		public IServiceBusPolicy Policy { get; set; }
        public IThreadActivityFactory ThreadActivityFactory { get; set; }

        public bool HasIdempotenceTracker
        {
            get { return IdempotenceTracker != null; }
        }

        public bool HasSubscriptionManager
        {
            get { return subscriptionManager != null; }
        }

        public ISubscriptionManager SubscriptionManager
		{
			get
			{
				if (!HasSubscriptionManager)
				{
					throw new SubscriptionManagerException(ESBResources.NoSubscriptionManager);
				}

				return subscriptionManager;
			}
			set { subscriptionManager = value; }
		}

		public bool HasInbox
		{
			get { return Inbox != null; }
		}

		public bool HasOutbox
		{
			get { return Outbox != null; }
		}

		public bool HasControlInbox
		{
			get { return ControlInbox != null; }
		}

		public bool HasServiceBusSection
		{
			get { return ServiceBusSection != null; }
		}

		public bool RemoveMessagesNotHandled { get; set; }
        public string EncryptionAlgorithm { get; set; }
        public string CompressionAlgorithm { get; set; }

		public IWorkerAvailabilityManager WorkerAvailabilityManager { get; private set; }

		public IPipelineFactory PipelineFactory { get; set; }

        public IServiceBusTransactionScopeFactory TransactionScopeFactory { get; set; }

		public IServiceBus StartServiceBus()
		{
			return new ServiceBus(this);
		}

		public IEncryptionAlgorithm FindEncryptionAlgorithm(string name)
		{
			return encryptionAlgorithms.Find(algorithm => algorithm.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		public void AddEncryptionAlgorithm(IEncryptionAlgorithm algorithm)
		{
			Guard.AgainstNull(algorithm, "algorithm");

			encryptionAlgorithms.Add(algorithm);
		}

		public ICompressionAlgorithm FindCompressionAlgorithm(string name)
		{
			return compressionAlgorithms.Find(algorithm => algorithm.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		public void AddCompressionAlgorithm(ICompressionAlgorithm algorithm)
		{
			Guard.AgainstNull(algorithm, "algorithm");

			compressionAlgorithms.Add(algorithm);
		}

		public bool IsWorker
		{
			get { return Worker != null; }
		}

		public string OutgoingEncryptionAlgorithm { get; internal set; }
		public string OutgoingCompressionAlgorithm { get; internal set; }
	}
}