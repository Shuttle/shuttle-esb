using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
    public class MemoryInboxTest : InboxFixture
    {
        [Test]
        public void Should_be_able_handle_errors_without_journal()
        {
            TestInboxError("memory://.", false, false);
        }

        [Test]
        public void Should_be_able_handle_errors_with_journal()
        {
            TestInboxError("memory://.", true, false);
        }

        [Test]
        public void Should_be_able_to_process_queue_timeously_without_journal()
        {
            TestInboxThroughput("memory://.", 2500, 1000, false, false);
        }

        [Test]
        public void Should_be_able_to_process_queue_timeously_with_journal()
        {
            TestInboxThroughput("memory://.", 2000, 1000, true, false);
        }

        [Test]
        public void Should_be_able_to_process_messages_concurrently_without_journal()
        {
            TestInboxConcurrency("memory://.", false, 200, false);
        }

        [Test]
        public void Should_be_able_to_process_messages_concurrently_with_journal()
        {
            TestInboxConcurrency("memory://.", true, 200, false);
        }
    }
}