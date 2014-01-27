using System;
using System.IO;
using NUnit.Framework;
using Shuttle.ESB.RabbitMQ;

namespace Shuttle.ESB.Test.Integration.RabbitMQ
{
	[TestFixture]
	public class RabbitMQQueueTest : IntegrationFixture
	{
		protected override void TearDownTest()
		{
			inboxQueue.Drop();
			outboxQueue.Drop();
		}

		private RabbitMQQueue inboxQueue;
		private RabbitMQQueue outboxQueue;

		protected override void TestSetUp()
		{
			base.TestSetUp();

			var inboxUri = new Uri("rabbitmq://shuttle:shuttle!@localhost/sit-inbox");
			var outboxUri = new Uri("rabbitmq://shuttle:shuttle!@localhost/sit-outbox");

			var configuration = new RabbitMQConfiguration();

			inboxQueue = new RabbitMQQueue(inboxUri, configuration);
			outboxQueue = new RabbitMQQueue(outboxUri, configuration);
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
	}
}