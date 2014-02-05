using System;
using NUnit.Framework;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration
{
    [TestFixture]
    public class ControlInboxServiceBusSection : ServiceBusSectionFixture
    {
        [Test]
        public void Should_be_able_to_load_a_full_configuration()
        {
            var section = GetServiceBusSection("Control-Full.config");

            Assert.IsNotNull(section);

			Assert.AreEqual("msmq://./control-inbox-work", section.ControlInbox.WorkQueueUri);
			Assert.AreEqual("msmq://./control-inbox-error", section.ControlInbox.ErrorQueueUri);
         

            Assert.AreEqual(25, section.ControlInbox.ThreadCount);
            Assert.AreEqual(25, section.ControlInbox.MaximumFailureCount);

            Assert.AreEqual(TimeSpan.FromMilliseconds(250), section.ControlInbox.DurationToSleepWhenIdle[0]);
            Assert.AreEqual(TimeSpan.FromSeconds(10), section.ControlInbox.DurationToSleepWhenIdle[1]);
            Assert.AreEqual(TimeSpan.FromSeconds(30), section.ControlInbox.DurationToSleepWhenIdle[2]);

            Assert.AreEqual(TimeSpan.FromMinutes(30), section.ControlInbox.DurationToIgnoreOnFailure[0]);
            Assert.AreEqual(TimeSpan.FromHours(1), section.ControlInbox.DurationToIgnoreOnFailure[1]);
        }
    }
}