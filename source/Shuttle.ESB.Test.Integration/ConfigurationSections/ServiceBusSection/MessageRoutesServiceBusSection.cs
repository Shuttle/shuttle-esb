using System;
using NUnit.Framework;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration
{
    [TestFixture]
    public class MessageRoutesServiceBusSection : ServiceBusSectionFixture
    {
        [Test]
		[TestCase("MessageRoutes.config")]
		[TestCase("MessageRoutes-Grouped.config")]
        public void Should_be_able_to_load_the_configuration(string file)
        {
            var section = GetServiceBusSection(file);

            Assert.IsNotNull(section);
            Assert.AreEqual(2, section.MessageRoutes.Count);

            foreach (MessageRouteElement map in section.MessageRoutes)
            {
                Console.WriteLine(map.Uri);

                foreach (SpecificationElement specification in map)
                {
                    Console.WriteLine("-> {0} - {1}", specification.Name, specification.Value);
                }
                
                Console.WriteLine();
            }
        }
    }
}