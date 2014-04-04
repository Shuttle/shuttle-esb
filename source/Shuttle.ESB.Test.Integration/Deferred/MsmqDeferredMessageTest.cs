using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration.Deferred
{
	public class MsmqDeferredMessageTest : DeferredFixture
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
		public void Should_be_able_to_perform_full_processing(bool useJournal, bool isTransactionalEndpoint, bool isTransactionalQueue)
		{
			var queueUriFormat = string.Concat("msmq://./{0}?transactional=", isTransactionalQueue);

			TestDeferredProcessing(string.Concat(queueUriFormat, "&journal=", useJournal), string.Concat(queueUriFormat, "&journal=", useJournal), string.Concat(queueUriFormat, "&journal=false"), isTransactionalEndpoint);
		}
	}
}