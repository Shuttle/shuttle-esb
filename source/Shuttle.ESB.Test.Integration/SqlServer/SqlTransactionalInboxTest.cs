using NUnit.Framework;
using Shuttle.ESB.SqlServer.Idempotence;

namespace Shuttle.ESB.Test.Integration
{
	public class SqlTransactionalInboxTest : InboxFixture
	{
		[Test]
		public void Should_be_able_handle_errors_without_journal()
		{
			TestInboxError("sql://shuttle", false, true);
		}

		[Test]
		public void Should_be_able_handle_errors_with_journal()
		{
			TestInboxError("sql://shuttle", true, true);
		}

		[Test]
		public void Should_be_able_to_process_queue_timeously_without_journal()
		{
			TestInboxThroughput("sql://shuttle", 200, 1000, false, true);
		}

		[Test]
		public void Should_be_able_to_process_queue_timeously_with_journal()
		{
			TestInboxThroughput("sql://shuttle", 200, 1000, true, true);
		}

		[Test]
		public void Should_be_able_to_process_messages_concurrently_without_journal()
		{
			TestInboxConcurrency("sql://shuttle", false, 500, true);
		}

		[Test]
		public void Should_be_able_to_process_messages_concurrently_with_journal()
		{
			TestInboxConcurrency("sql://shuttle", true, 500, true);
		}

		[Test]
		public void Should_be_able_to_process_queue_timeously_without_journal_with_idempotence_tracked()
		{
			try
			{
				ConfigurationComplete += OnConfigurationComplete;

				TestInboxThroughput("sql://shuttle", 200, 1000, false, true);
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

				TestInboxThroughput("sql://shuttle", 200, 1000, true, true);
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