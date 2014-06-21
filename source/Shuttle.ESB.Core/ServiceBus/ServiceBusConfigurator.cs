using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class ServiceBusConfigurator
	{
		private readonly ServiceBusConfiguration configuration = new ServiceBusConfiguration();

		public ServiceBusConfigurator MessageSerializer(ISerializer serializer)
		{
			Guard.AgainstNull(serializer, "serializer");

			configuration.Serializer = serializer;

			return this;
		}

		public ServiceBusConfigurator MessageHandlerFactory(IMessageHandlerFactory messageHandlerFactory)
		{
			Guard.AgainstNull(messageHandlerFactory, "messageHandlerFactory");

			configuration.MessageHandlerFactory = messageHandlerFactory;

			return this;
		}

		public ServiceBusConfigurator AddCompressionAlgorithm(ICompressionAlgorithm algorithm)
		{
			Guard.AgainstNull(algorithm, "algorithm");

			configuration.AddCompressionAlgorithm(algorithm);

			return this;
		}

		public ServiceBusConfigurator AddEnryptionAlgorithm(IEncryptionAlgorithm algorithm)
		{
			Guard.AgainstNull(algorithm, "algorithm");

			configuration.AddEncryptionAlgorithm(algorithm);

			return this;
		}

		public ServiceBusConfigurator SubscriptionManager(ISubscriptionManager manager)
		{
			Guard.AgainstNull(manager, "manager");

			configuration.SubscriptionManager = manager;

			return this;
		}

		public ServiceBusConfigurator AddModule(IModule module)
		{
			Guard.AgainstNull(module, "module");

			configuration.Modules.Add(module);

			return this;
		}

		public ServiceBusConfigurator Policy(IServiceBusPolicy policy)
		{
			Guard.AgainstNull(policy, "policy");

			configuration.Policy = policy;

			return this;
		}

		public ServiceBusConfigurator ThreadActivityFactory(IThreadActivityFactory factory)
		{
			Guard.AgainstNull(factory, "factory");

			configuration.ThreadActivityFactory = factory;

			return this;
		}

		public IServiceBusConfiguration Configuration()
		{
			return configuration;
		}

		public ServiceBusConfigurator PipelineFactory(IPipelineFactory pipelineFactory)
		{
			Guard.AgainstNull(pipelineFactory, "pipelineFactory");

			configuration.PipelineFactory = pipelineFactory;

			return this;
		}

		public ServiceBusConfigurator TransactionScopeFactory(
			ITransactionScopeFactory transactionScopeFactory)
		{
			Guard.AgainstNull(transactionScopeFactory, "transactionScopeFactory");

			configuration.TransactionScopeFactory = transactionScopeFactory;

			return this;
		}

		public ServiceBusConfigurator MessageRouteProvider(IMessageRouteProvider messageRouteProvider)
		{
			Guard.AgainstNull(messageRouteProvider, "messageRouteProvider");

			configuration.MessageRouteProvider = messageRouteProvider;

			return this;
		}

		public ServiceBusConfigurator ForwardingRouteProvider(IMessageRouteProvider forwardingRouteProvider)
		{
			Guard.AgainstNull(forwardingRouteProvider, "forwardingRouteProvider");

			configuration.ForwardingRouteProvider = forwardingRouteProvider;

			return this;
		}
	}
}