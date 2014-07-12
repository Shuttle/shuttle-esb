using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	public class SqlResourceUsageTest : ResourceUsageFixture
	{
		[Test]
		[TestCase(false)]
		[TestCase(true)]
		public void Should_not_exceeed_normal_resource_usage(bool isTransactionalEndpoint)
		{
			const string queueUriFormat = "sql://shuttle/{0}";

			TestResourceUsage(queueUriFormat, queueUriFormat, isTransactionalEndpoint);
		}
	}
}