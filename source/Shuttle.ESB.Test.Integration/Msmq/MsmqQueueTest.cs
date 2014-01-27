using System;
using System.IO;
using System.Transactions;
using NUnit.Framework;
using Shuttle.ESB.Msmq;

namespace Shuttle.ESB.Test.Integration.Msmq
{
	[TestFixture]
	public class MsmqQueueTest : IntegrationFixture
	{
		protected override void TearDownTest()
		{
			inboxQueue.Drop();
			outboxQueue.Drop();
		}

		private MsmqQueue inboxQueue;
		private MsmqQueue outboxQueue;

		protected override void TestSetUp()
		{
			base.TestSetUp();

			var inboxUri = new Uri("msmq://./sit-inbox?transactional=true");
			var outboxUri = new Uri("msmq://./sit-outbox?transactional=true");

			var configuration = new MsmqConfiguration();

			inboxQueue = new MsmqQueue(inboxUri, configuration);
			outboxQueue = new MsmqQueue(outboxUri, configuration);

			inboxQueue.Create();
			outboxQueue.Create();
		}

		[Test]
		public void Should_be_able_to_remove_a_message()
		{
			var messageId = Guid.NewGuid();

			inboxQueue.Enqueue(messageId, new MemoryStream());

			inboxQueue.Remove(messageId);

			Assert.IsNull(inboxQueue.Dequeue());
		}

		[Test]
		public void Should_be_able_to_send_a_message_to_and_receive_a_message_from_a_queue()
		{
			var stream = new MemoryStream();

			stream.WriteByte(100);

			inboxQueue.Enqueue(Guid.NewGuid(), stream);

			var dequeued = inboxQueue.Dequeue();

			Assert.AreEqual(100, dequeued.ReadByte());
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
	}
}