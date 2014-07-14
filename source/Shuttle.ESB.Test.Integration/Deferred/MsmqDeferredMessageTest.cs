using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration.Deferred
{
	public class MsmqDeferredMessageTest : DeferredFixture
	{
		[Test]
		[TestCase(false)]
		[TestCase(true)]
		public void Should_be_able_to_perform_full_processing(bool isTransactionalEndpoint)
		{
			TestDeferredProcessing(@"msmq://./{0}", isTransactionalEndpoint);
		}
	}
}