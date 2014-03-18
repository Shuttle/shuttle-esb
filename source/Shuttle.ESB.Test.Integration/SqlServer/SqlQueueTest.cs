using System;
using System.IO;
using System.Transactions;
using NUnit.Framework;
using Shuttle.ESB.SqlServer;

namespace Shuttle.ESB.Test.Integration.SqlServer
{
	[TestFixture]
	public class SqlQueueTest : IntegrationFixture
	{
		protected override void TearDownTest()
		{
			inboxQueue.Drop();
			outboxQueue.Drop();
		}

		private SqlQueue inboxQueue;
		private SqlQueue outboxQueue;

		protected override void TestSetUp()
		{
			inboxQueue = new SqlQueue(new Uri("sql://shuttle/queue-inbox"));
			outboxQueue = new SqlQueue(new Uri("sql://shuttle/queue-outbox"));

			inboxQueue.Create();
			outboxQueue.Create();

			inboxQueue.Purge();
			outboxQueue.Purge();
		}

		[Test]
		public void Should_be_able_to_remove_a_message()
		{
			var messagId = Guid.NewGuid();

			inboxQueue.Enqueue(messagId, new MemoryStream());

			Assert.IsNull(inboxQueue.Dequeue());
		}

		[Test]
		public void Should_be_able_to_rollback_message_sent_in_transaction()
		{
			using (new TransactionScope())
			{
				inboxQueue.Enqueue(Guid.NewGuid(), new MemoryStream());
			}

			Assert.AreEqual(0, inboxQueue.Count);

			inboxQueue.Enqueue(Guid.NewGuid(), new MemoryStream());

			Assert.AreEqual(1, inboxQueue.Count);
		}

		[Test]
		public void Should_be_able_to_enqueue_and_dequeue_a_message()
		{
			var stream = new MemoryStream();

			stream.WriteByte(100);

			inboxQueue.Enqueue(Guid.NewGuid(), stream);

			var dequeued = inboxQueue.Dequeue();

			Assert.AreEqual(100, dequeued.ReadByte());
		}


		[Test]
		public void Should_be_able_journal_messages_until_acknowledged()
		{
			var queue = new SqlQueue(new Uri("sql://shuttle/queue-inbox"));
			var messageId = Guid.NewGuid();
			var stream = new MemoryStream();

			queue.Enqueue(messageId, stream);

			Assert.NotNull(queue.Dequeue());
			Assert.Null(queue.Dequeue());

			queue = new SqlQueue(new Uri("sql://shuttle/queue-inbox"));

			Assert.NotNull(queue.Dequeue());
			Assert.Null(queue.Dequeue());

			queue.Acknowledge(messageId);

			queue = new SqlQueue(new Uri("sql://shuttle/queue-inbox"));

			Assert.Null(queue.Dequeue());
		}
	}
}