using System;
using System.Collections.Generic;
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

		private static ServiceBusSection _serviceBusSection;

		private readonly List<IEncryptionAlgorithm> _encryptionAlgorithms = new List<IEncryptionAlgorithm>();
		private readonly List<ICompressionAlgorithm> _compressionAlgorithms = new List<ICompressionAlgorithm>();

		private ISerializer _serializer;
		private ISubscriptionManager _subscriptionManager;

		public ServiceBusConfiguration()
		{
			WorkerAvailabilityManager = new WorkerAvailabilityManager();
			Modules = new ModuleCollection();
			TransactionScope = new TransactionScopeConfiguration();
			DeferredMessageConfiguration = new DeferredMessageConfiguration();
		}

		public static ServiceBusSection ServiceBusSection
		{
			get
			{
				return _serviceBusSection ??
				       (_serviceBusSection = ConfigurationManager.GetSection(ServiceBusSectionName) as ServiceBusSection);
			}
		}

		public ISerializer Serializer
		{
			get { return _serializer; }
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

				_serializer = value;
			}
		}

		public IInboxQueueConfiguration Inbox { get; set; }
		public IControlInboxQueueConfiguration ControlInbox { get; set; }
		public IOutboxQueueConfiguration Outbox { get; set; }
		public IWorkerConfiguration Worker { get; set; }
		public ITransactionScopeConfiguration TransactionScope { get; set; }

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
			get { return _subscriptionManager != null; }
		}

		public ISubscriptionManager SubscriptionManager
		{
			get
			{
				if (!HasSubscriptionManager)
				{
					throw new SubscriptionManagerException(ESBResources.NoSubscriptionManager);
				}

				return _subscriptionManager;
			}
			set { _subscriptionManager = value; }
		}

		public bool HasDeferredMessageQueue
		{
			get { return DeferredMessageConfiguration.DeferredMessageQueue != null; }
		}

		public IDeferredMessageConfiguration DeferredMessageConfiguration { get; set; }

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
			return
				_encryptionAlgorithms.Find(algorithm => algorithm.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		public void AddEncryptionAlgorithm(IEncryptionAlgorithm algorithm)
		{
			Guard.AgainstNull(algorithm, "algorithm");

			_encryptionAlgorithms.Add(algorithm);
		}

		public ICompressionAlgorithm FindCompressionAlgorithm(string name)
		{
			return
				_compressionAlgorithms.Find(algorithm => algorithm.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		public void AddCompressionAlgorithm(ICompressionAlgorithm algorithm)
		{
			Guard.AgainstNull(algorithm, "algorithm");

			_compressionAlgorithms.Add(algorithm);
		}

		public bool IsWorker
		{
			get { return Worker != null; }
		}

		public string OutgoingEncryptionAlgorithm { get; internal set; }
		public string OutgoingCompressionAlgorithm { get; internal set; }


		public IServiceBusConfiguration QueueFactory<TQueueFactory>(object configuration)
			where TQueueFactory : IQueueFactory
		{
			var queueFactory = Activator.CreateInstance(typeof (TQueueFactory), configuration) as IQueueFactory;
			QueueManager.Instance.RegisterQueueFactory(queueFactory);
			return this;
		}
	}
}