using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	public class RabbitMQInboxTest : InboxFixture
	{
		[Test]
		[TestCase(false, false)]
		[TestCase(true, false)]
		[TestCase(false, true)]
		[TestCase(true, true)]
		[TestCase(false, true)]
		[TestCase(true, true)]
		[TestCase(false, false)]
		[TestCase(true, false)]
		public void Should_be_able_handle_errors(bool useJournal, bool isTransactionalEndpoint)
		{
			TestInboxError("rabbitmq://shuttle:shuttle!@localhost", useJournal, isTransactionalEndpoint);
		}

		[Test]
		[TestCase(250, false, false)]
		[TestCase(500, true, false)]
		[TestCase(250, false, true)]
		[TestCase(500, true, true)]
		[TestCase(250, false, true)]
		[TestCase(500, true, true)]
		[TestCase(250, false, false)]
		[TestCase(500, true, false)]
		public void Should_be_able_to_process_messages_concurrently(int msToComplete, bool useJournal,
		                                                            bool isTransactionalEndpoint)
		{
			TestInboxConcurrency("rabbitmq://shuttle:shuttle!@localhost", 250, false, false);
		}

		[Test]
		[TestCase(350, false, false, false)]
		[TestCase(35, false, true, false)]
		[TestCase(350, false, false, true)]
		[TestCase(35, false, true, true)]
		[TestCase(350, false, false, true)]
		[TestCase(35, false, true, true)]
		[TestCase(350, false, false, false)]
		[TestCase(35, false, true, false)]
		[TestCase(35, true, false, false)]
		[TestCase(20, true, true, false)]
		[TestCase(35, true, false, true)]
		[TestCase(20, true, true, true)]
		[TestCase(35, true, false, true)]
		[TestCase(20, true, true, true)]
		[TestCase(35, true, false, false)]
		[TestCase(20, true, true, false)]
		public void Should_be_able_to_process_queue_timeously_without_journal(int count, bool useIdempotenceTracker,
		                                                                      bool useJournal, bool isTransactionalEndpoint)
		{
			TestInboxThroughput("rabbitmq://shuttle:shuttle!@localhost", 1000, count, useIdempotenceTracker, useJournal, isTransactionalEndpoint);
		}

		[Test]
		public void Should_be_able_to_handle_a_deferred_message()
		{
			TestInboxDeferred("rabbitmq://shuttle:shuttle!@localhost");
		}
	}
}