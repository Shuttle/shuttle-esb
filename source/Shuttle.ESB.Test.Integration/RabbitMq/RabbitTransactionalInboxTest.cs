using System;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.RabbitMq;

namespace Shuttle.ESB.Test.Integration.RabbitMq
{
	public class RabbitTransactionalInboxTest : InboxFixture
	{

		public void SetUp(bool isTransactional, bool isDurable)
		{
			var factory = QueueManager.Instance.GetQueueFactory("rabbitmq://") as RabbitMqQueueFactory;

			if (factory == null)
			{
				return;
			}

			factory.Configuration.AddQueueConfiguration(new RabbitMqQueueConfiguration(new Uri("rabbitmq://./test-inbox-work"), isTransactional, isDurable));
			factory.Configuration.AddQueueConfiguration(new RabbitMqQueueConfiguration(new Uri("rabbitmq://./test-inbox-journal"), isTransactional, isDurable));
			factory.Configuration.AddQueueConfiguration(new RabbitMqQueueConfiguration(new Uri("rabbitmq://./test-error"), isTransactional, isDurable));
		}

		public void TearDown()
		{
			var factory = QueueManager.Instance.GetQueueFactory("rabbitmq://") as RabbitMqQueueFactory;

			if (factory == null)
			{
				return;
			}

			factory.Configuration.RemoveQueueConfiguration(new Uri("rabbitmq://./test-inbox-work"));
			factory.Configuration.RemoveQueueConfiguration(new Uri("rabbitmq://./test-inbox-journal"));
			factory.Configuration.RemoveQueueConfiguration(new Uri("rabbitmq://./test-error"));
		}

		[Test]
		[TestCase(false, false, false, false)]
		[TestCase(true, false, false, true)]
		[TestCase(false, true, true, true)]
		[TestCase(true, true, true, true)]
		[TestCase(false, true, false, true)]
		[TestCase(true, true, false, true)]
		[TestCase(false, false, true, true)]
		[TestCase(true, false, true, true)]
		public void Should_be_able_handle_errors(bool useJournal, bool isTransactionalEndpoint, bool isTransactionalQueue, bool isDurable)
		{
			SetUp(isTransactionalQueue, isDurable);
			TestInboxError("rabbitmq://.", useJournal, isTransactionalEndpoint);
			TearDown();
		}

		[Test]
		[TestCase(250, false, false, false, true)]
		[TestCase(500, true, false, false, true)]
		[TestCase(250, false, true, true, true)]
		[TestCase(500, true, true, true, true)]
		[TestCase(250, false, true, false, true)]
		[TestCase(500, true, true, false, true)]
		[TestCase(250, false, false, true, true)]
		[TestCase(500, true, false, true, true)]
		public void Should_be_able_to_process_messages_concurrently(int msToComplete, bool useJournal, bool isTransactionalEndpoint, bool isTransactionalQueue, bool isDurable)
		{
			SetUp(isTransactionalQueue, isDurable);
			TestInboxConcurrency("rabbitmq://.", 250, false, false);
			TearDown();
		}

		[Test]
		[TestCase(350, false, false, false, false, true)]
		[TestCase(35, false, true, false, false, true)]
		[TestCase(350, false, false, true, true, true)]
		[TestCase(35, false, true, true, true, true)]
		[TestCase(350, false, false, true, false, true)]
		[TestCase(35, false, true, true, false, true)]
		[TestCase(350, false, false, false, true, true)]
		[TestCase(35, false, true, false, true, true)]
		[TestCase(35, true, false, false, false, true)]
		[TestCase(20, true, true, false, false, true)]
		[TestCase(35, true, false, true, true, true)]
		[TestCase(20, true, true, true, true, true)]
		[TestCase(35, true, false, true, false, true)]
		[TestCase(20, true, true, true, false, true)]
		[TestCase(35, true, false, false, true, true)]
		[TestCase(20, true, true, false, true, true)]
		public void Should_be_able_to_process_queue_timeously_without_journal(int count, bool useIdempotenceTracker, bool useJournal, bool isTransactionalEndpoint, 
			bool isTransactionalQueue, bool isDurable)
		{
			SetUp(isTransactionalQueue, isDurable);
			TestInboxThroughput("rabbitmq://.", 1000, count, useIdempotenceTracker, useJournal, isTransactionalEndpoint);
			TearDown();
		}
	}
}