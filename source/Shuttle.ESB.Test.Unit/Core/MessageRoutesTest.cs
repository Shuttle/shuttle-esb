using System;
using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared.Mocks;

namespace Shuttle.ESB.Test.Unit.Core
{
    public class MessageRoutesTest : UnitFixture
    {
        [Test]
        public void Should_be_able_to_create_new_routes()
        {
            var queue = new MemoryQueue(new Uri("memory://."));
            var route = new MessageRoute(queue);
            var routes = new MessageRouteCollection();

            route.AddSpecification(new RegexMessageRouteSpecification("simple"));

            routes.Add(route);

			Assert.AreSame(queue, routes.FindAll(new SimpleCommand().GetType().FullName)[0].Queue);
        }
    }
}
