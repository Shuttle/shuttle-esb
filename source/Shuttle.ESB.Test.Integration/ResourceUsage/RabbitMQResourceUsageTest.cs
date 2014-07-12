using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	public class RabbitMQResourceUsageTest : ResourceUsageFixture
	{
		[Test]
		[TestCase(false)]
		[TestCase(true)]
		public void Should_not_exceeed_normal_resource_usage(bool isTransactionalEndpoint)
		{
			const string queueUriFormat = "rabbitmq://shuttle:shuttle!@localhost/{0}";

			TestResourceUsage(queueUriFormat, queueUriFormat, isTransactionalEndpoint);
		}
	}
}