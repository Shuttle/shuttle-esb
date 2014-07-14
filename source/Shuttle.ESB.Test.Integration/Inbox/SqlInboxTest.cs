using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	public class SqlInboxTest : InboxFixture
	{
		[Test]
		[TestCase(false)]
		[TestCase(true)]
		public void Should_be_able_handle_errors(bool isTransactionalEndpoint)
		{
			TestInboxError("sql://shuttle/{0}", isTransactionalEndpoint);
		}

		[Test]
		[TestCase(500, false)]
		[TestCase(500, true)]
		public void Should_be_able_to_process_messages_concurrently(int msToComplete, bool isTransactionalEndpoint)
		{
			TestInboxConcurrency("sql://shuttle/{0}", msToComplete, false);
		}

		[Test]
		[TestCase(200, false)]
		[TestCase(200, true)]
		public void Should_be_able_to_process_queue_timeously_without_journal(int count, bool isTransactionalEndpoint)
		{
			TestInboxThroughput("sql://shuttle/{0}", 1000, count, isTransactionalEndpoint);
		}
	}
}