namespace Shuttle.ESB.Core
{
	public interface IServiceBusConfigurationBuilder
	{
        IServiceBusConfigurationBuilder TransactionScopeFactory(ITransactionScopeFactory transactionScopeFactory);
        IServiceBusConfigurationBuilder PipelineFactory(IPipelineFactory pipelineFactory);
        IServiceBusConfigurationBuilder MessageRouteProvider(IMessageRouteProvider messageRouteProvider);
        IServiceBusConfigurationBuilder MessageHandlerFactory(IMessageHandlerFactory messageHandlerFactory);
        IServiceBusConfigurationBuilder MessageSerializer(ISerializer serializer);
        IServiceBusConfigurationBuilder ForwardingRouteProvider(IMessageRouteProvider forwardingRouteProvider);
        IServiceBusConfigurationBuilder AddEnryptionAlgorithm(IEncryptionAlgorithm algorithm);
		IServiceBusConfigurationBuilder AddCompressionAlgorithm(ICompressionAlgorithm algorithm);
		IServiceBusConfigurationBuilder SubscriptionManager(ISubscriptionManager manager);
		IServiceBusConfigurationBuilder AddModule(IModule module);
		IServiceBusConfigurationBuilder Policy(IServiceBusPolicy policy);
		IServiceBusConfigurationBuilder ThreadActivityFactory(IThreadActivityFactory factory);
		IServiceBus Start();
		IServiceBusConfiguration Configuration();
	}
}