using System;
using NUnit.Framework;

namespace Shuttle.ESB.Test.Integration
{
    [TestFixture]
    public class OutboxServiceBusSection : ServiceBusSectionFixture
    {
        [Test]
        public void Should_be_able_to_load_a_full_configuration()
        {
            var section = GetServiceBusSection("Outbox-Full.config");

            Assert.IsNotNull(section);

            Assert.AreEqual("msmq://./outbox_work", section.Outbox.WorkQueueUri);
            Assert.AreEqual("msmq://./outbox_error", section.Outbox.ErrorQueueUri);

            Assert.AreEqual(25, section.Outbox.MaximumFailureCount);

            Assert.AreEqual(TimeSpan.FromMilliseconds(250), section.Outbox.DurationToSleepWhenIdle[0]);
            Assert.AreEqual(TimeSpan.FromSeconds(10), section.Outbox.DurationToSleepWhenIdle[1]);
            Assert.AreEqual(TimeSpan.FromSeconds(30), section.Outbox.DurationToSleepWhenIdle[2]);

            Assert.AreEqual(TimeSpan.FromMinutes(30), section.Outbox.DurationToIgnoreOnFailure[0]);
            Assert.AreEqual(TimeSpan.FromHours(1), section.Outbox.DurationToIgnoreOnFailure[1]);
        }

        [Test]
        public void Should_be_able_to_load_with_only_required_configuration()
        {
            var section = GetServiceBusSection("Outbox-Required.config");

            Assert.IsNotNull(section);

            Assert.AreEqual("msmq://./outbox_work", section.Outbox.WorkQueueUri);
            Assert.AreEqual("msmq://./outbox_error", section.Outbox.ErrorQueueUri);
        }
    }
}