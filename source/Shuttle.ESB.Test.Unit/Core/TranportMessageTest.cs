using System;
using NUnit.Framework;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Unit.Core
{
    public class TranportMessageTest : UnitFixture
    {
        [Test]
        public void Should_be_able_to_determine_if_message_should_be_ignored()
        {
            var messsage = new TransportMessage()
                           {
                               IgnoreTillDate = DateTime.Now.AddMinutes(1)
                           };

            Assert.IsTrue(messsage.Ignore());

            messsage.IgnoreTillDate = DateTime.Now.AddMilliseconds(-1);

            Assert.IsFalse(messsage.Ignore());
        }

        [Test]
        public void Should_be_able_to_register_failures_and_have_IgnoreTillDate_set()
        {
            var message = new TransportMessage();

            var before = DateTime.Now;

            message.RegisterFailure("failure");

            Assert.IsTrue(before <= message.IgnoreTillDate);

            message = new TransportMessage();

            var durationToIgnoreOnFailure =
                new[]
                {
                    TimeSpan.FromMinutes(3),
                    TimeSpan.FromMinutes(30),
                    TimeSpan.FromHours(2)
                };

            Assert.IsFalse(DateTime.Now.AddMinutes(3) <= message.IgnoreTillDate);

            message.RegisterFailure("failure", durationToIgnoreOnFailure[0]);

            Assert.IsTrue(DateTime.Now.AddMinutes(3) <= message.IgnoreTillDate);
            Assert.IsFalse(DateTime.Now.AddMinutes(30) <= message.IgnoreTillDate);

            message.RegisterFailure("failure", durationToIgnoreOnFailure[1]);

            Assert.IsTrue(DateTime.Now.AddMinutes(30) <= message.IgnoreTillDate);
            Assert.IsFalse(DateTime.Now.AddHours(2) <= message.IgnoreTillDate);

            message.RegisterFailure("failure", durationToIgnoreOnFailure[2]);

            Assert.IsTrue(DateTime.Now.AddHours(2) <= message.IgnoreTillDate);
        }
    }
}