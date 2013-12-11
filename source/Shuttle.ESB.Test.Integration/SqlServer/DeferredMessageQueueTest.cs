using System;
using System.IO;
using System.Transactions;
using NUnit.Framework;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.SqlServer;
using Shuttle.ESB.Test.Shared;

namespace Shuttle.ESB.Test.Integration.SqlServer
{
	public class DeferredMessageQueueTest : Fixture
	{
		protected override void TestSetUp()
		{
			((IPurge)DeferredMessageQueue.Default()).Purge();
		}

		[Test]
		public void Should_be_able_to_rollback_message_sent_in_transaction()
		{
			var queue = DeferredMessageQueue.Default();
			var count = (ICount)queue;

			using (new TransactionScope())
			{
				queue.Enqueue(DateTime.Now, new MemoryStream());
			}

			Assert.AreEqual(0, count.Count);

			queue.Enqueue(DateTime.Now, new MemoryStream());

			Assert.AreEqual(1, count.Count);
		}


		[Test]
		public void Should_be_able_to_enqueue_and_dequeue_a_message()
		{
			var queue = DeferredMessageQueue.Default();
			var stream = new MemoryStream();

			stream.WriteByte(100);

			queue.Enqueue(DateTime.Now.AddDays(-1), stream);

			var dequeued = queue.Dequeue(DateTime.Now);

			Assert.AreEqual(100, dequeued.ReadByte());
		}
	}
}