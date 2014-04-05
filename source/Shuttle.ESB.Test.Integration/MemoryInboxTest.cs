using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	public class MemoryInboxTest : InboxFixture
	{
		[Test]
		public void Should_be_able_handle_errors_without_journal()
		{
			TestInboxError("memory://./{0}", false);
		}

		[Test]
		public void Should_be_able_handle_errors_with_journal()
		{
			TestInboxError("memory://./{0}", false);
		}

		[Test]
		public void Should_be_able_to_process_queue_timeously_without_journal()
		{
			TestInboxThroughput("memory://./{0}", 1000, 1000, false);
		}

		[Test]
		public void Should_be_able_to_process_queue_timeously_with_journal()
		{
			TestInboxThroughput("memory://./{0}", 1000, 1000, false);
		}

		[Test]
		public void Should_be_able_to_process_messages_concurrently_without_journal()
		{
			TestInboxConcurrency("memory://./{0}", 200, false);
		}

		[Test]
		public void Should_be_able_to_process_messages_concurrently_with_journal()
		{
			TestInboxConcurrency("memory://./{0}", 200, false);
		}
	}
}