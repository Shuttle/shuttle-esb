using System.Transactions;
using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	[TestFixture]
	public class SharedServiceBusSection : ServiceBusSectionFixture
	{
		[Test]
		[TestCase("Shared.config")]
		[TestCase("Shared-Grouped.config")]
		public void Should_be_able_to_load_shared_configuration(string file)
		{
			var section = GetServiceBusSection(file);

			Assert.IsNotNull(section);

			Assert.IsTrue(section.RemoveMessagesNotHandled);
			Assert.AreEqual("GZip", section.CompressionAlgorithm);
			Assert.AreEqual("3DES", section.EncryptionAlgorithm);
			Assert.IsNotNull(section.TransactionScope);
			Assert.AreEqual(IsolationLevel.ReadCommitted, section.TransactionScope.IsolationLevel);
			Assert.AreEqual(15, section.TransactionScope.TimeoutSeconds);
			Assert.IsFalse(section.TransactionScope.Enabled);
		}
	}
}