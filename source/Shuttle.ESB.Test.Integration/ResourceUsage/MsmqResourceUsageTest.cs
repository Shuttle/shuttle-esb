using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	public class MsmqResourceUsageTest : ResourceUsageFixture
	{
		[Test]
		[TestCase(false, false, false)]
		[TestCase(true, false, false)]
		[TestCase(false, true, true)]
		[TestCase(true, true, true)]
		[TestCase(false, true, false)]
		[TestCase(true, true, false)]
		[TestCase(false, false, true)]
		[TestCase(true, false, true)]
		public void Should_not_exceeed_normal_resource_usage(bool useJournal, bool isTransactionalEndpoint, bool isTransactionalQueue)
		{
			var queueUriFormat = string.Concat("msmq://./{0}?transactional=", isTransactionalQueue);

			TestResourceUsage(string.Concat(queueUriFormat, "&journal=", useJournal), queueUriFormat, isTransactionalEndpoint);
		}
	}
}