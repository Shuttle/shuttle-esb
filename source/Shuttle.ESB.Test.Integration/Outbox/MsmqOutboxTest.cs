using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	public class MsmqOutboxTest : OutboxFixture
	{
		[Test]
		[TestCase(false, false, false)]
		[TestCase(true, false, false)]
		[TestCase(false, true, false)]
		[TestCase(true, true, false)]
		[TestCase(true, false, true)]
		[TestCase(false, false, true)]
		[TestCase(false, true, true)]
		[TestCase(true, true, true)]
		public void Should_be_able_handle_errors(bool useJournal, bool isTransactionalEndpoint, bool isTransactionalQueue)
		{
			var queueUriFormat = string.Concat("msmq://./{0}?transactional=", isTransactionalQueue);

			TestOutboxSending(string.Concat(queueUriFormat, "&journal=", useJournal), isTransactionalEndpoint);
		}
	}
}