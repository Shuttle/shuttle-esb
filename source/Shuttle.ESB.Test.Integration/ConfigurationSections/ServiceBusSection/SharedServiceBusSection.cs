using System;
using System.Transactions;
using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
    [TestFixture]
    public class SharedServiceBusSection : ServiceBusSectionFixture
    {
        [Test]
        public void Should_be_able_to_load_shared_configuration()
        {
            var section = GetServiceBusSection("Shared.config");

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