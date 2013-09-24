using System;
using NUnit.Framework;
using Rhino.Mocks;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Test.Integration.Core
{
    public class ThreadActivityTest : IntegrationFixture
    {
        [Test]
        public void Should_be_able_to_have_the_thread_wait()
        {
            var activity = new ThreadActivity(new[]
                                                  {
                                                      TimeSpan.FromMilliseconds(250),
                                                      TimeSpan.FromMilliseconds(500)
                                                  });

            var start = DateTime.Now;

            var mockState = Mock<IActiveState>();

            mockState.Stub(mock => mock.Active).Return(true);

            activity.Waiting(mockState);

            Assert.IsTrue((DateTime.Now - start).TotalMilliseconds >= 250);

            activity.Waiting(mockState);

            Assert.IsTrue((DateTime.Now - start).TotalMilliseconds >= 750);
        }
    }
}