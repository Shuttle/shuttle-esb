using System;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.RabbitMQ;

namespace Shuttle.ESB.Test.Unit.RabbitMQ
{
    [TestFixture]
    public class RabbitMQQueueFactoryTest : UnitFixture
    {
        private static IQueueFactory SUT()
        {
            return new RabbitMQQueueFactory();
        }

        [Test]
        public void Should_be_able_to_create_a_new_queue_from_a_given_uri()
        {
            Assert.NotNull(SUT().Create(new Uri("rabbitmq://shuttle:shuttle!@localhost/work-queue")));
        }
    }
}
