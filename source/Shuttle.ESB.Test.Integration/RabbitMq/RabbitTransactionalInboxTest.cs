using System;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.RabbitMq;

namespace Shuttle.ESB.Test.Integration.RabbitMq
{
	public class RabbitTransactionalInboxTest : InboxFixture
	{

		public void SetUp(bool isTransactional)
		{
			var factory = QueueManager.Instance.GetQueueFactory("rabbitmq://") as RabbitMqQueueFactory;

			if (factory == null)
			{
				return;
			}

			var queue =	factory.Configuration
				.DeclareQueue(new Uri("rabbitmq://./test-inbox-work"))
				.IsTransactional(isTransactional)
				.OverwriteIfExists

				.DeclareQueue(new Uri("rabbitmq://./test-inbox-journal"))
				.IsTransactional(isTransactional)
				.OverwriteIfExists

				.DeclareQueue(new Uri("rabbitmq://./test-error"))
				.IsTransactional(isTransactional)
				.OverwriteIfExists;
			}

		public void TearDown()
		{
			var factory = QueueManager.Instance.GetQueueFactory("rabbitmq://") as RabbitMqQueueFactory;

			if (factory == null)
			{
				return;
			}

			factory.Configuration.RemoveAll();
		}

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
			SetUp(isTransactionalQueue);
			TestInboxError("rabbitmq://.", useJournal, isTransactionalEndpoint);
			TearDown();
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
		public void Should_be_able_to_process_messages_concurrently(int msToComplete, bool useJournal, bool isTransactionalEndpoint, bool isTransactionalQueue)
		{
			SetUp(isTransactionalQueue);
			TestInboxConcurrency("rabbitmq://.", 250, false, false);
			TearDown();
		}

		[Test]
		[TestCase(350, false, false, false, false)]
		[TestCase(35, false, true, false, false)]
		[TestCase(350, false, false, true, true)]
		[TestCase(35, false, true, true, true)]
		[TestCase(350, false, false, true, false)]
		[TestCase(35, false, true, true, false)]
		[TestCase(350, false, false, false, true)]
		[TestCase(35, false, true, false, true)]
		[TestCase(35, true, false, false, false)]
		[TestCase(20, true, true, false, false)]
		[TestCase(35, true, false, true, true)]
		[TestCase(20, true, true, true, true)]
		[TestCase(35, true, false, true, false)]
		[TestCase(20, true, true, true, false)]
		[TestCase(35, true, false, false, true)]
		[TestCase(20, true, true, false, true)]
		public void Should_be_able_to_process_queue_timeously_without_journal(int count, bool useIdempotenceTracker, bool useJournal, bool isTransactionalEndpoint, 
			bool isTransactionalQueue)
		{
			SetUp(isTransactionalQueue);
			TestInboxThroughput("rabbitmq://.", 1000, count, useIdempotenceTracker, useJournal, isTransactionalEndpoint);
			TearDown();
		}
	}
}