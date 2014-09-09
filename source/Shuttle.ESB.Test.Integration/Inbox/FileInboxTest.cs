using System.Threading;
using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	public class FileInboxTest : InboxFixture
	{
		[Test]
		[TestCase(false)]
		[TestCase(true)]
		public void Should_be_able_handle_errors(bool isTransactionalEndpoint)
		{
			TestInboxError(FileMQExtensions.FileUri(), isTransactionalEndpoint);
		}

		[Test]
		[TestCase(100, false)]
		[TestCase(100, true)]
		public void Should_be_able_to_process_messages_concurrently(int msToComplete, bool isTransactionalEndpoint)
		{
			TestInboxConcurrency(FileMQExtensions.FileUri(), msToComplete, isTransactionalEndpoint);
		}

		[Test]
		[TestCase(100, false)]
		[TestCase(100, true)]
		public void Should_be_able_to_process_queue_timeously(int count, bool isTransactionalEndpoint)
		{
			TestInboxThroughput(FileMQExtensions.FileUri(), 1000, count, isTransactionalEndpoint);
		}

		[Test]
		public void Should_be_able_to_handle_a_deferred_message()
		{
			TestInboxDeferred(FileMQExtensions.FileUri());
		}
	}
}