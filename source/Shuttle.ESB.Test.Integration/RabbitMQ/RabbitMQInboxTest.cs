using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	public class RabbitMQInboxTest : InboxFixture
	{
		[TestCase(true)]
		[TestCase(false)]
		public void Should_be_able_handle_errors(bool isTransactionalEndpoint)
		{
			TestInboxError("rabbitmq://shuttle:shuttle!@localhost/{0}", false, isTransactionalEndpoint);
		}

		[TestCase(250, false)]
		[TestCase(250, true)]
		public void Should_be_able_to_process_messages_concurrently(int msToComplete, bool isTransactionalEndpoint)
		{
			TestInboxConcurrency("rabbitmq://shuttle:shuttle!@localhost/{0}", msToComplete, false, isTransactionalEndpoint);
		}

		[TestCase(350, true, true)]
		[TestCase(350, true, true)]
		[TestCase(350, false, true)]
		[TestCase(350, false, false)]
		public void Should_be_able_to_process_queue_timeously(int count, bool useIdempotenceTracker, bool isTransactionalEndpoint)
		{
			TestInboxThroughput("rabbitmq://shuttle:shuttle!@localhost/{0}", 1000, count, useIdempotenceTracker, false, isTransactionalEndpoint);
		}

		[Test]
		public void Should_be_able_to_handle_a_deferred_message()
		{
			TestInboxDeferred("rabbitmq://shuttle:shuttle!@localhost/{0}");
		}
	}
}