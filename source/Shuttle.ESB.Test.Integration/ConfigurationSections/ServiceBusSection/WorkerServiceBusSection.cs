using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	[TestFixture]
	public class WorkerServiceBusSection : ServiceBusSectionFixture
	{
		[Test]
		[TestCase("Worker.config")]
		[TestCase("Worker-Grouped.config")]
		public void Should_be_able_to_load_a_valid_configuration(string file)
		{
			var section = GetServiceBusSection(file);

			Assert.IsNotNull(section);
			Assert.AreEqual("msmq://./distributor-server-control-inbox-work", section.Worker.DistributorControlWorkQueueUri);
			Assert.AreEqual(5, section.Worker.ThreadAvailableNotificationIntervalSeconds);
		}

		[Test]
		public void Should_be_able_to_load_an_empty_configuration()
		{
			var section = GetServiceBusSection("Empty.config");

			Assert.IsNotNull(section);
			Assert.IsEmpty(section.Worker.DistributorControlWorkQueueUri);
			Assert.AreEqual(15, section.Worker.ThreadAvailableNotificationIntervalSeconds);
		}
	}
}