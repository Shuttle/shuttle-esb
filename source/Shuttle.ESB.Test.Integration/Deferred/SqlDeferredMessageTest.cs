using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration.Deferred
{
	public class SqlDeferredMessageTest : DeferredFixture
	{
		[Test]
		[TestCase(false)]
		[TestCase(true)]
		public void Should_be_able_to_perform_full_processing(bool isTransactionalEndpoint)
		{
			TestDeferredProcessing("sql://shuttle/{0}", isTransactionalEndpoint);
		}
	}
}