using System;
using NUnit.Framework;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Unit.Core
{
    public class MemoryQueueFactoryTest : UnitFixture
    {
        private static IQueueFactory SUT()
        {
            return new MemoryQueueFactory();
        }

        [Test]
        public void Should_be_able_to_create_a_new_queue_from_a_given_uri()
        {
            Assert.NotNull(SUT().Create(new Uri("memory://./inputqueue")));
        }
    }
}

