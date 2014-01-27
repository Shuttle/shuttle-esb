using System;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Msmq;

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
			TestInboxError(string.Concat("msmq://./{0}", isTransactionalQueue ? "?transactional=true" : ""), useJournal, isTransactionalEndpoint);
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
			TestInboxConcurrency(string.Concat("msmq://./{0}", isTransactionalQueue ? "?transactional=true" : ""), msToComplete, useJournal, isTransactionalEndpoint);
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
		public void Should_be_able_to_process_queue_timeously_without_journal(int count, bool useIdempotenceTracker, bool useJournal, bool isTransactionalEndpoint, bool isTransactionalQueue)
		{
			TestInboxThroughput(string.Concat("msmq://./{0}", isTransactionalQueue ? "?transactional=true" : ""), 1000, count, useIdempotenceTracker, useJournal, isTransactionalEndpoint);
		}

		[Test]
		public void Should_be_able_to_handle_a_deferred_message()
		{
			TestInboxDeferred("msmq://./{0}");
		}
	}
}