using NUnit.Framework;
using Shuttle.ESB.SqlServer.Idempotence;

namespace Shuttle.ESB.Test.Integration.RabbitMq
{
    public class RabbitTransactionalInboxTest : InboxFixture
    {
        [Test]
        public void Should_be_able_handle_errors_without_journal()
        {
            TestInboxError("rabbitmq://.", false, true);
        }

        [Test]
        public void Should_be_able_handle_errors_with_journal()
        {
					TestInboxError("rabbitmq://.", true, true);
        }

        [Test]
        public void Should_be_able_to_process_queue_timeously_without_journal()
        {
					TestInboxThroughput("rabbitmq://.", 350, 1000, false, true);
        }

        [Test]
        public void Should_be_able_to_process_queue_timeously_with_journal()
        {
					TestInboxThroughput("rabbitmq://.", 35, 1000, true, true);
        }

        [Test]
        public void Should_be_able_to_process_messages_concurrently_without_journal()
        {
					TestInboxConcurrency("rabbitmq://.", false, 250, true);
        }

        [Test]
        public void Should_be_able_to_process_messages_concurrently_with_journal()
        {
					TestInboxConcurrency("rabbitmq://.", true, 500, true);
        }

        [Test]
        public void Should_be_able_to_process_queue_timeously_without_journal_with_idempotence_tracked()
        {
            try
            {
                ConfigurationComplete += OnConfigurationComplete;

								TestInboxThroughput("rabbitmq://.", 35, 1000, false, true);
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

								TestInboxThroughput("rabbitmq://.", 20, 1000, true, true);
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