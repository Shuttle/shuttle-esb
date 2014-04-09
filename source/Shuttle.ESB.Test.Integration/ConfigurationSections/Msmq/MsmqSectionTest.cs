using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	[TestFixture]
	public class MsmqSectionTest : MsmqSectionFixture
	{
		[Test]
		public void Should_be_able_to_load_a_full_configuration()
		{
			var section = GetMsmqSection("Msmq.config");

			Assert.IsNotNull(section);

			Assert.AreEqual(1500, section.LocalQueueTimeoutMilliseconds);
			Assert.AreEqual(3500, section.RemoteQueueTimeoutMilliseconds);
		}
	}
}