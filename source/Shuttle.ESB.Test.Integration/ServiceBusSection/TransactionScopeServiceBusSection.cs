using System.Transactions;
using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
	[TestFixture]
	public class TransactionScopeServiceBusSection : ServiceBusSectionFixture
	{
		[Test]
		public void Should_be_able_to_load_a_valid_configuration()
		{
			var section = GetServiceBusSection("TransactionScope.config");

			Assert.IsNotNull(section);
			Assert.IsFalse(section.TransactionScope.Enabled);
			Assert.AreEqual(IsolationLevel.RepeatableRead, section.TransactionScope.IsolationLevel);
			Assert.AreEqual(300, section.TransactionScope.TimeoutSeconds);
		}

		[Test]
		public void Should_be_able_to_load_an_empty_configuration()
		{
			var section = GetServiceBusSection("Empty.config");

			Assert.IsTrue(section.TransactionScope.Enabled);
			Assert.AreEqual(IsolationLevel.ReadCommitted, section.TransactionScope.IsolationLevel);
			Assert.AreEqual(30, section.TransactionScope.TimeoutSeconds);
		}
	}
}