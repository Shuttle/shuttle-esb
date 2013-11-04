using System;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Msmq;
using Shuttle.ESB.SqlServer.Idempotence;

namespace Shuttle.ESB.Test.Integration
{
	public class MsmqNonTransactionalInboxTest : InboxFixture
	{
		[SetUp]
		public void MsmqSetUp()
		{
			var factory = QueueManager.Instance.GetQueueFactory("msmq://") as MsmqQueueFactory;

			if (factory == null)
			{
				return;
			}

			factory.Configuration.AddQueueConfiguration(new MsmqQueueConfiguration(new Uri("msmq://./test-inbox-work"), false));
			factory.Configuration.AddQueueConfiguration(new MsmqQueueConfiguration(new Uri("msmq://./test-inbox-journal"), false));
			factory.Configuration.AddQueueConfiguration(new MsmqQueueConfiguration(new Uri("msmq://./test-error"), false));
		}

		[TearDown]
		public void MsmqTearDown()
		{
			var factory = QueueManager.Instance.GetQueueFactory("msmq://") as MsmqQueueFactory;

			if (factory == null)
			{
				return;
			}

			factory.Configuration.RemoveQueueConfiguration(new Uri("msmq://./test-inbox-work"));
			factory.Configuration.RemoveQueueConfiguration(new Uri("msmq://./test-inbox-journal"));
			factory.Configuration.RemoveQueueConfiguration(new Uri("msmq://./test-error"));
		}

		[Test]
		public void Should_be_able_handle_errors_without_journal()
		{
			TestInboxError("msmq://.", false, false);
		}

		[Test]
		public void Should_be_able_handle_errors_with_journal()
		{
			TestInboxError("msmq://.", true, false);
		}
		[Test]
		public void Should_be_able_to_process_messages_concurrently_without_journal()
		{
			TestInboxConcurrency("msmq://.", false, 250, false);
		}

		[Test]
		public void Should_be_able_to_process_messages_concurrently_with_journal()
		{
			TestInboxConcurrency("msmq://.", true, 500, false);
		}

		[Test]
		public void Should_be_able_to_process_queue_timeously_without_journal()
		{
			TestInboxThroughput("msmq://.", 350, 1000, false, false);
		}

		[Test]
		public void Should_be_able_to_process_queue_timeously_with_journal()
		{
			TestInboxThroughput("msmq://.", 35, 1000, true, false);
		}

		[Test]
		public void Should_be_able_to_process_queue_timeously_without_journal_with_idempotence_tracked()
		{
			try
			{
				ConfigurationComplete += OnConfigurationComplete;

				TestInboxThroughput("msmq://.", 35, 1000, false, false);
			}
			finally
			{
				ConfigurationComplete -= OnConfigurationComplete;
			}
		}

		[Test]
		public void Should_be_able_to_process_queue_timeously_with_journal_with_idempotence_tracked()
		{
			try
			{
				ConfigurationComplete += OnConfigurationComplete;

				TestInboxThroughput("msmq://.", 20, 1000, true, false);
			}
			finally
			{
				ConfigurationComplete -= OnConfigurationComplete;
			}
		}

		private static void OnConfigurationComplete(object sender, ConfigurationEventArgs configurationEventArgs)
		{
			configurationEventArgs.Configuration.IdempotenceTracker = IdempotenceTracker.Default();
		}
	}
}