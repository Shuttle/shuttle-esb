using System;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Msmq;

namespace Shuttle.ESB.Test.Unit.Msmq
{
    [TestFixture]
    public class MsmqQueueFactoryTest : UnitFixture
    {
        private static IQueueFactory SUT()
        {
            return new MsmqQueueFactory();
        }

        [Test]
        public void Should_be_able_to_create_a_new_queue_from_a_given_uri()
        {
            Assert.NotNull(SUT().Create(new Uri("msmq://./inputqueue")));
        }
    }
}
