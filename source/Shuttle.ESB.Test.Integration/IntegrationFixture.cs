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

		protected static ServiceBusConfiguration DefaultConfiguration(bool isTransactional)
		{
			var configuration = new ServiceBusConfiguration
				{
					Serializer = new DefaultSerializer(),
					MessageHandlerFactory = new DefaultMessageHandlerFactory(),
					PipelineFactory = new DefaultPipelineFactory(),
					ForwardingRouteProvider = new DefaultForwardingRouteProvider(),
					TransactionScopeFactory = new DefaultServiceBusTransactionScopeFactory(),
					Policy = new DefaultServiceBusPolicy(),
					ThreadActivityFactory = new DefaultThreadActivityFactory(),
					TransactionScope = new TransactionScopeConfiguration
						{
							Enabled = isTransactional
						}
				};

			return configuration;
		}

		protected void AttemptDropQueues(string workQueueUriFormat, string errorQueueUriFormat)
		{
			using (var queueManager = QueueManager.Default())
			{
				queueManager.GetQueue(string.Format(workQueueUriFormat, "test-inbox-work")).AttemptDrop();
				queueManager.GetQueue(string.Format(errorQueueUriFormat, "test-error")).AttemptDrop();
			}
		}
	}
}