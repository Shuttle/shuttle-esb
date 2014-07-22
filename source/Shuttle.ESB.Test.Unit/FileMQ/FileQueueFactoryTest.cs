using System;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.FileMQ;

namespace Shuttle.ESB.Test.Unit.FileMQ
{
    [TestFixture]
    public class FileQueueFactoryTest : UnitFixture
    {
        private static IQueueFactory SUT()
        {
            return new FileQueueFactory();
        }

        [Test]
        public void Should_be_able_to_create_a_new_queue_from_a_given_uri()
        {
            Assert.NotNull(SUT().Create(new Uri("filemq://./work-queue")));
        }
    }
}
