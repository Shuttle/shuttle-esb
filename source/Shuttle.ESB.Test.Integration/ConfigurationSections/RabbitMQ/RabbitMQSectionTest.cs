using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	[TestFixture]
	public class RabbitMQSectionTest : RabbitMQSectionFixture
	{
		[Test]
		public void Should_be_able_to_load_a_full_configuration()
		{
			var section = GetRabbitMQSection("RabbitMQ.config");

			Assert.IsNotNull(section);

			Assert.AreEqual(50, section.RequestedHeartbeat);
			Assert.AreEqual(1500, section.LocalQueueTimeoutMilliseconds);
			Assert.AreEqual(3500, section.RemoteQueueTimeoutMilliseconds);
			Assert.AreEqual(1500, section.ConnectionCloseTimeoutMilliseconds);
			Assert.AreEqual(5, section.OperationRetryCount);
			Assert.AreEqual(100, section.DefaultPrefetchCount);
		}
	}
}