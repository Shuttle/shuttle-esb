using System;
using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration.ConfigurationFile
{
    [TestFixture]
    public class SharedConfigurationTest : ConfigurationTestFixture
    {
        [Test]
        public void Should_be_able_to_load_shared_configuration()
        {
            var section = GetServiceBusSection("Shared.config");

            Assert.IsNotNull(section);

            Assert.IsTrue(section.RemoveMessagesNotHandled);
            Assert.AreEqual("GZip", section.CompressionAlgorithm);
            Assert.AreEqual("3DES", section.EncryptionAlgorithm);
        }
    }
}