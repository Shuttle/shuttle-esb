using System;
using NUnit.Framework;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration
{
    [TestFixture]
    public class QueueFactoriesServiceBusSection : ServiceBusSectionFixture
    {
        [Test]
		[TestCase("QueueFactories.config")]
		[TestCase("QueueFactories-Grouped.config")]
        public void Should_be_able_to_load_the_configuration(string file)
        {
            var section = GetServiceBusSection(file);

            Assert.IsNotNull(section);
            Assert.IsNotNull(section.QueueFactories);
			Assert.IsFalse(section.QueueFactories.Scan);
			Assert.AreEqual(2, section.QueueFactories.Count);

            foreach (QueueFactoryElement queueFactoryElement in section.QueueFactories)
            {
                Console.WriteLine(queueFactoryElement.Type);
            }
        }

		[Test]
		[TestCase("QueueFactories-Missing.config")]
        public void Should_be_able_to_handle_missing_element(string file)
        {
            var section = GetServiceBusSection(file);

            Assert.IsNotNull(section);
            Assert.IsNotNull(section.QueueFactories);
			Assert.IsTrue(section.QueueFactories.Scan);
        }

		[Test]
		[TestCase("QueueFactories-EmptyTypes.config")]
        public void Should_be_able_to_handle_empty_types(string file)
        {
            var section = GetServiceBusSection(file);

            Assert.IsNotNull(section);
            Assert.IsNotNull(section.QueueFactories);
			Assert.IsTrue(section.QueueFactories.Scan);
        }
    }
}