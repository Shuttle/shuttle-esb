using System;
using System.IO;
using System.Transactions;
using NUnit.Framework;
using Shuttle.ESB.RabbitMq;

namespace Shuttle.ESB.Test.Integration.RabbitMq
{
	[TestFixture]
	public class RabbitMqQueueTest : IntegrationFixture
	{
		private RabbitMqQueueFactory _factory;

		protected override void TestTearDown()
		{
			inboxQueue.Drop();
			outboxQueue.Drop();
			_factory.Dispose();
		}

		private RabbitMqQueue inboxQueue;
		private RabbitMqQueue outboxQueue;

		protected override void TestSetUp()
		{
			base.TestSetUp();

			_factory = new RabbitMqQueueFactory();
			inboxQueue = _factory.Create(new Uri("rabbitmq://localhost/rmq_inbox")) as RabbitMqQueue;
			outboxQueue = _factory.Create(new Uri("rabbitmq://./rmq_outbox")) as RabbitMqQueue;

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
		public void Should_be_able_to_send_and_receive_a_message_to_a_queue()
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
				inboxQueue.Enqueue(Guid.NewGuid(), new MemoryStream());
			Assert.AreEqual(0, inboxQueue.Count);

			inboxQueue.Enqueue(Guid.NewGuid(), new MemoryStream());
			Assert.AreEqual(1, inboxQueue.Count);
		}
	}
}