using System;
using NUnit.Framework;
using Shuttle.ESB.SqlServer.Idempotence;

namespace Shuttle.ESB.Test.Integration
{
    public class SqlInboxTest : InboxFixture
    {
        [Test]
        public void Should_be_able_handle_errors_without_journal()
        {
            TestInboxError("sql://shuttle", false);
        }

        [Test]
        public void Should_be_able_handle_errors_with_journal()
        {
            TestInboxError("sql://shuttle", true);
        }

        [Test]
        public void Should_be_able_to_process_queue_timeously_without_journal()
        {
            TestInboxThroughput("sql://shuttle", 200, 1000, false);
        }

        [Test]
        public void Should_be_able_to_process_queue_timeously_with_journal()
        {
            TestInboxThroughput("sql://shuttle", 200, 1000, true);
        }

        [Test]
        public void Should_be_able_to_process_messages_concurrently_without_journal()
        {
            TestInboxConcurrency("sql://shuttle", false, 500);
        }

        [Test]
        public void Should_be_able_to_process_messages_concurrently_with_journal()
        {
            TestInboxConcurrency("sql://shuttle", true, 500);
        }

        [Test]
        public void Should_be_able_to_process_queue_timeously_without_journal_with_idempotence_tracked()
        {
            try
            {
                ConfigurationComplete += OnConfigurationComplete;

                TestInboxThroughput("sql://shuttle", 200, 1000, false);
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

                TestInboxThroughput("sql://shuttle", 200, 1000, true);
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