using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	public class MsmqInboxTest : InboxFixture
	{
		[Test]
		[TestCase(false)]
		[TestCase(true)]
		public void Should_be_able_handle_errors(bool isTransactionalEndpoint)
		{
			TestInboxError("msmq://./{0}", isTransactionalEndpoint);
		}

		[Test]
		[TestCase(300, false)]
		[TestCase(300, true)]
		public void Should_be_able_to_process_messages_concurrently(int msToComplete, bool isTransactionalEndpoint)
		{
			TestInboxConcurrency("msmq://./{0}", msToComplete, isTransactionalEndpoint);
		}

		[Test]
		[TestCase(300, false)]
		[TestCase(300, true)]
		public void Should_be_able_to_process_queue_timeously(int count, bool isTransactionalEndpoint)
		{
			TestInboxThroughput("msmq://./{0}", 1000, count, isTransactionalEndpoint);
		}

		[Test]
		public void Should_be_able_to_handle_a_deferred_message()
		{
			TestInboxDeferred("msmq://./{0}");
		}
	}
}