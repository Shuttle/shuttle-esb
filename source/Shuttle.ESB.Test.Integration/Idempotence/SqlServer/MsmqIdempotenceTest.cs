using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration.Idempotence.SqlServer.Msmq
{
	[TestFixture]
	public class MsmqIdempotenceTest : IdempotenceFixture
	{
		[Test]
		[TestCase(false, false, false, false)]
		[TestCase(true, false, false, false)]
		[TestCase(false, true, true, false)]
		[TestCase(true, true, true, false)]
		[TestCase(false, true, false, false)]
		[TestCase(true, true, false, false)]
		[TestCase(false, false, true, false)]
		[TestCase(true, false, true, false)]
		[TestCase(false, false, false, true)]
		[TestCase(true, false, false, true)]
		[TestCase(false, true, true, true)]
		[TestCase(true, true, true, true)]
		[TestCase(false, true, false, true)]
		[TestCase(true, true, false, true)]
		[TestCase(false, false, true, true)]
		[TestCase(true, false, true, true)]
		public void Should_be_able_to_perform_full_processing(bool useJournal, bool isTransactionalEndpoint,
		                                                      bool isTransactionalQueue, bool enqueueUniqueMessages)
		{
			var queueUriFormat = string.Concat("msmq://./{0}?transactional=", isTransactionalQueue);

			TestIdempotenceProcessing(string.Concat(queueUriFormat, "&journal=", useJournal), queueUriFormat, isTransactionalEndpoint,
			                   enqueueUniqueMessages);
		}
	}
}