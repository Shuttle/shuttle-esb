using System;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Msmq;
using Shuttle.ESB.RabbitMq;

namespace Shuttle.ESB.Test.Unit.RabbitMq
{
    [TestFixture]
	public class RabbitMqQueueFactoryTest : UnitFixture
    {
        private static IQueueFactory SUT()
        {
            return new RabbitMqQueueFactory();
        }

        [Test]
        public void Should_be_able_to_create_a_new_queue_from_a_given_uri()
        {
            Assert.NotNull(SUT().Create(new Uri("rabbitmq://./inputqueue")));
        }
    }
}
