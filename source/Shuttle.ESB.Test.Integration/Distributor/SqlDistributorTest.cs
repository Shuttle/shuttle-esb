using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration.Deferred
{
	public class SqlDistributorTest : DistributorFixture
	{
		[Test]
		[TestCase(false)]
		[TestCase(true)]
		public void Should_be_able_to_distribute_messages(bool isTransactionalEndpoint)
		{
			TestDistributor(@"sql://shuttle/{0}", isTransactionalEndpoint);
		}
	}
}