using System.Linq;
using NUnit.Framework;
using Shuttle.ESB.Msmq;

namespace Shuttle.ESB.Test.Integration.Msmq
{
	[TestFixture]
	public class MsmqSectionTest : MsmqSectionFixture
	{
		[Test]
		public void Should_be_able_to_load_a_full_configuration()
		{
			var section = GetMsmqSection("Msmq.config");

			Assert.IsNotNull(section);
			Assert.AreEqual(2, section.Queues.Count);

			var queues = section.Queues.Cast<MsmqQueueElement>().ToList();

			Assert.AreEqual("msmq://./inbox-work-1", queues.ElementAt(0).Uri);
			Assert.IsTrue(queues.ElementAt(0).IsTransactional);
			Assert.AreEqual("msmq://./inbox-work-2", queues.ElementAt(1).Uri);
			Assert.IsFalse(queues.ElementAt(1).IsTransactional);
		}
	}
}