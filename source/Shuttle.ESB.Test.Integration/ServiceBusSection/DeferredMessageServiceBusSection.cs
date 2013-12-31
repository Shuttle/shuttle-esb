using System;
using NUnit.Framework;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration
{
    [TestFixture]
    public class DeferredMessageServiceBusSection : ServiceBusSectionFixture
    {
        [Test]
        public void Should_be_able_to_load_a_full_configuration()
        {
            var section = GetServiceBusSection("DeferredMessage.config");

            Assert.IsNotNull(section);

            Assert.AreEqual(TimeSpan.FromMilliseconds(250), section.DeferredMessage.DurationToSleepWhenIdle[0]);
			Assert.AreEqual(TimeSpan.FromSeconds(1), section.DeferredMessage.DurationToSleepWhenIdle[1]);
			Assert.AreEqual(TimeSpan.FromSeconds(3), section.DeferredMessage.DurationToSleepWhenIdle[2]);
		}
    }
}