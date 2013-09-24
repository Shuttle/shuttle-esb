using System;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.SqlServer;

namespace Shuttle.ESB.Test.Unit.SqlServer
{
    public class SqlQueueTest : UnitFixture
    {
        [Test]
        public void Should_throw_exception_when_trying_to_create_a_queue_with_incorrect_scheme()
        {
            Assert.Throws<InvalidSchemeException>(() => new SqlQueue(new Uri("msmq://./inputqueue")));
        }
        
    }
}
