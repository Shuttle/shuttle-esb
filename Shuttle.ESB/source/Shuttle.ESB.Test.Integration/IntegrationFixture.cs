using log4net;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Infrastructure.Log4Net;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared;

namespace Shuttle.ESB.Test.Integration
{
	public class IntegrationFixture : Fixture
	{
		protected IServiceBus GetServiceBus()
		{
			return ServiceBus
				.Create()
				.Start();
		}

		protected override void FixtureSetUp()
		{
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(IntegrationFixture))));
		}

		protected static ServiceBusConfiguration DefaultConfiguration()
		{
			var configuration = ConfigurationBuilder.Build();

			configuration.Serializer = new DefaultSerializer();
			configuration.MessageHandlerFactory = new DefaultMessageHandlerFactory();
			configuration.PipelineFactory = new DefaultPipelineFactory();
			configuration.ForwardingRouteProvider = new DefaultForwardingRouteProvider();
		    configuration.TransactionScopeFactory = new DefaultServiceBusTransactionScopeFactory();
		    configuration.Policy = new DefaultServiceBusPolicy();
		    configuration.ThreadActivityFactory = new DefaultThreadActivityFactory();

			return configuration;
		}
	}
}