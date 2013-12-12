using System.Collections.Generic;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	internal class ServiceBusConfigurationBuilder : IServiceBusConfigurationBuilder
	{
		private readonly ServiceBusConfiguration configuration = new ServiceBusConfiguration();

		public ServiceBusConfigurationBuilder()
		{
			var tasks = new List<RegistrationTask>
				{
					new RegisterSharedConfigurationTask(),
					new RegisterDeferredMessageConfigurationTask(),
					new RegisterControlInboxQueueConfigurationTask(),
					new RegisterInboxQueueConfigurationTask(),
					new RegisterOutboxQueueConfigurationTask(),
					new RegisterWorkerConfigurationTask()
				};

			tasks.ForEach(task => task.Execute(configuration));

			DefaultMessageSerializer();
			DefaultMessageHandlerFactory();
			DefaultMessageRouteProvider();
			DefaultForwardingRouteProvider();
			DefaultPipelineFactory();
			DefaultTransactionScopeFactory();
			DefaultServiceBusPolicy();
			DefaultThreadActivityFactory();
		}

		public IServiceBusConfigurationBuilder MessageSerializer(ISerializer serializer)
		{
			Guard.AgainstNull(serializer, "serializer");

			configuration.Serializer = serializer;

			return this;
		}

		public IServiceBusConfigurationBuilder DefaultMessageSerializer()
		{
			configuration.Serializer = new DefaultSerializer();

			return this;
		}

		public IServiceBusConfigurationBuilder MessageHandlerFactory(IMessageHandlerFactory messageHandlerFactory)
		{
			Guard.AgainstNull(messageHandlerFactory, "messageHandlerFactory");

			configuration.MessageHandlerFactory = messageHandlerFactory;

			return this;
		}

		public IServiceBusConfigurationBuilder DefaultMessageHandlerFactory()
		{
			configuration.MessageHandlerFactory = new DefaultMessageHandlerFactory();

			return this;
		}

		public IServiceBusConfigurationBuilder AddCompressionAlgorithm(ICompressionAlgorithm algorithm)
		{
			Guard.AgainstNull(algorithm, "algorithm");

			configuration.AddCompressionAlgorithm(algorithm);

			return this;
		}

		public IServiceBusConfigurationBuilder OutgoingEncryptionAlgorithm(string name)
		{
			Guard.AgainstNullOrEmptyString(name, "name");

			configuration.OutgoingEncryptionAlgorithm = name;

			return this;
		}

		public IServiceBusConfigurationBuilder OutgoingCompressionAlgorithm(string name)
		{
			Guard.AgainstNullOrEmptyString(name, "name");

			configuration.OutgoingCompressionAlgorithm = name;

			return this;
		}

		public IServiceBusConfigurationBuilder AddEnryptionAlgorithm(IEncryptionAlgorithm algorithm)
		{
			Guard.AgainstNull(algorithm, "algorithm");

			configuration.AddEncryptionAlgorithm(algorithm);

			return this;
		}

		public IServiceBusConfigurationBuilder SubscriptionManager(ISubscriptionManager manager)
		{
			Guard.AgainstNull(manager, "manager");

			configuration.SubscriptionManager = manager;

			return this;
		}

		public IServiceBusConfigurationBuilder DeferredMessageQueue(IDeferredMessageQueue deferredMessageQueue)
		{
			Guard.AgainstNull(deferredMessageQueue, "deferredMessageQueue");

			configuration.DeferredMessageConfiguration.DeferredMessageQueue = deferredMessageQueue;

			return this;
		}

		public IServiceBusConfigurationBuilder AddModule(IModule module)
		{
			Guard.AgainstNull(module, "module");

			configuration.Modules.Add(module);

			return this;
		}

		public IServiceBusConfigurationBuilder Policy(IServiceBusPolicy policy)
		{
			Guard.AgainstNull(policy, "policy");

			configuration.Policy = policy;

			return this;
		}

		public IServiceBusConfigurationBuilder ThreadActivityFactory(IThreadActivityFactory factory)
		{
			Guard.AgainstNull(factory, "factory");

			configuration.ThreadActivityFactory = factory;

			return this;
		}

		public IServiceBus Start()
		{
			return new ServiceBus(configuration).Start();
		}

		public IServiceBusConfiguration Configuration()
		{
			return configuration;
		}

		public IServiceBusConfigurationBuilder PipelineFactory(IPipelineFactory pipelineFactory)
		{
			Guard.AgainstNull(pipelineFactory, "pipelineFactory");

			configuration.PipelineFactory = pipelineFactory;

			return this;
		}

		public IServiceBusConfigurationBuilder DefaultPipelineFactory()
		{
			configuration.PipelineFactory = new DefaultPipelineFactory();

			return this;
		}

		public IServiceBusConfigurationBuilder TransactionScopeFactory(
			IServiceBusTransactionScopeFactory serviceBusTransactionScopeFactory)
		{
			Guard.AgainstNull(serviceBusTransactionScopeFactory, "serviceBusTransactionScopeFactory");

			configuration.TransactionScopeFactory = serviceBusTransactionScopeFactory;

			return this;
		}

		public IServiceBusConfigurationBuilder DefaultTransactionScopeFactory()
		{
			configuration.TransactionScopeFactory = new DefaultServiceBusTransactionScopeFactory();

			return this;
		}

		public IServiceBusConfigurationBuilder MessageRouteProvider(IMessageRouteProvider messageRouteProvider)
		{
			Guard.AgainstNull(messageRouteProvider, "messageRouteProvider");

			configuration.MessageRouteProvider = messageRouteProvider;

			return this;
		}

		public IServiceBusConfigurationBuilder DefaultMessageRouteProvider()
		{
			configuration.MessageRouteProvider = new DefaultMessageRouteProvider();

			return this;
		}

		public IServiceBusConfigurationBuilder ForwardingRouteProvider(IMessageRouteProvider forwardingRouteProvider)
		{
			Guard.AgainstNull(forwardingRouteProvider, "forwardingRouteProvider");

			configuration.ForwardingRouteProvider = forwardingRouteProvider;

			return this;
		}

		public IServiceBusConfigurationBuilder DefaultForwardingRouteProvider()
		{
			configuration.ForwardingRouteProvider = new DefaultForwardingRouteProvider();

			return this;
		}

		public IServiceBusConfigurationBuilder DefaultServiceBusPolicy()
		{
			configuration.Policy = new DefaultServiceBusPolicy();

			return this;
		}

		public IServiceBusConfigurationBuilder DefaultThreadActivityFactory()
		{
			configuration.ThreadActivityFactory = new DefaultThreadActivityFactory();

			return this;
		}
	}
}