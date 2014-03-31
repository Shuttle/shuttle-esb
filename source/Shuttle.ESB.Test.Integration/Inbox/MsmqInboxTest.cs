using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	public class MsmqInboxTest : InboxFixture
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
		public void Should_be_able_handle_errors(bool useJournal, bool isTransactionalEndpoint, bool isTransactionalQueue)
		{
			var queueUriFormat = string.Concat("msmq://./{0}?transactional=", isTransactionalQueue);

			TestInboxError(string.Concat(queueUriFormat, "&journal=", useJournal), queueUriFormat, isTransactionalEndpoint);
		}

		[Test]
		[TestCase(250, false, false, false)]
		[TestCase(500, true, false, false)]
		[TestCase(250, false, true, true)]
		[TestCase(500, true, true, true)]
		[TestCase(250, false, true, false)]
		[TestCase(500, true, true, false)]
		[TestCase(250, false, false, true)]
		[TestCase(500, true, false, true)]
		public void Should_be_able_to_process_messages_concurrently(int msToComplete, bool useJournal,
		                                                            bool isTransactionalEndpoint, bool isTransactionalQueue)
		{
			var queueUriFormat = string.Concat("msmq://./{0}?transactional=", isTransactionalQueue);

			TestInboxConcurrency(string.Concat(queueUriFormat, "&journal=", useJournal), queueUriFormat, msToComplete,
			                     isTransactionalEndpoint);
		}

		[Test]
		[TestCase(350, false, false, false)]
		[TestCase(35, false, false, false)]
		[TestCase(350, false, true, true)]
		[TestCase(35, false, true, true)]
		[TestCase(350, false, true, false)]
		[TestCase(35, false, true, false)]
		[TestCase(350, false, false, true)]
		[TestCase(35, false, false, true)]
		[TestCase(35, true, false, false)]
		[TestCase(20, true, false, false)]
		[TestCase(35, true, true, true)]
		[TestCase(20, true, true, true)]
		[TestCase(35, true, true, false)]
		[TestCase(20, true, true, false)]
		[TestCase(35, true, false, true)]
		[TestCase(20, true, false, true)]
		public void Should_be_able_to_process_queue_timeously_without_journal(int count, bool useJournal,
		                                                                      bool isTransactionalEndpoint,
		                                                                      bool isTransactionalQueue)
		{
			var queueUriFormat = string.Concat("msmq://./{0}?transactional=", isTransactionalQueue);

			TestInboxThroughput(string.Concat(queueUriFormat, "&journal=", useJournal), queueUriFormat, 1000, count,
			                    isTransactionalEndpoint);
		}

		[Test]
		public void Should_be_able_to_handle_a_deferred_message()
		{
			TestInboxDeferred("msmq://./{0}", "msmq://./{0}");
		}
	}
}