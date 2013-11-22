using System.Linq;
using NUnit.Framework;
using Shuttle.ESB.RabbitMq;

namespace Shuttle.ESB.Test.Integration.RabbitMq
{
	[TestFixture]
	public class RabbitMqSectionTest : RabbitMqSectionFixture
	{
		[Test]
		public void Should_be_able_to_load_a_full_configuration()
		{
			var section = GetRabbitMqSection("RabbitMq.config");

			Assert.IsNotNull(section);
			Assert.AreEqual(2, section.Queues.Count);

			var queues = section.Queues.Cast<RabbitMqQueueElement>().ToList();

			Assert.AreEqual("rabbitmq://./inbox-work-1", queues.ElementAt(0).Uri);
			Assert.IsTrue(queues.ElementAt(0).IsTransactional);
			Assert.AreEqual("rabbitmq://./inbox-work-2", queues.ElementAt(1).Uri);
			Assert.IsFalse(queues.ElementAt(1).IsTransactional);

			var exchanges = section.Exchanges.Cast<RabbitMqExchangeElement>().ToList();

			Assert.AreEqual("exchange-alpha", exchanges.ElementAt(0).Name);
			Assert.AreEqual("fanout", exchanges.ElementAt(0).Type);
			Assert.IsFalse(exchanges.ElementAt(0).IsDurable);
			Assert.IsTrue(exchanges.ElementAt(0).AutoDelete);

			Assert.AreEqual("exchange-omega", exchanges.ElementAt(1).Name);
			Assert.AreEqual("topic", exchanges.ElementAt(1).Type);
			Assert.IsTrue(exchanges.ElementAt(1).IsDurable);
			Assert.IsFalse(exchanges.ElementAt(1).AutoDelete);
		}
	}
}