using NUnit.Framework;
using Shuttle.ESB.Core;
using Shuttle.ESB.Test.Shared.Mocks;
using SomeNamespace;

namespace Shuttle.ESB.Test.Unit.Core
{
    [TestFixture]
    public class RouteSpecificationTest : UnitFixture
    {
        [Test]
        public void Should_be_able_to_use_regex()
        {
            var specification = new RegexMessageRouteSpecification("simple");

            Assert.IsFalse(specification.IsSatisfiedBy(new OtherNamespaceCommand()));
            Assert.IsTrue(specification.IsSatisfiedBy(new SimpleCommand()));
        }

        [Test]
        public void Should_be_able_to_use_starts_with()
        {
            var specification = new StartsWithMessageRouteSpecification("Shuttle.ESB");

            Assert.IsFalse(specification.IsSatisfiedBy(new OtherNamespaceCommand()));
            Assert.IsTrue(specification.IsSatisfiedBy(new SimpleCommand()));
        }

        [Test]
        public void Should_be_able_to_use_typelist()
        {
            var specification = new TypeListMessageRouteSpecification(typeof (SimpleCommand));

            Assert.IsFalse(specification.IsSatisfiedBy(new OtherNamespaceCommand()));
            Assert.IsTrue(specification.IsSatisfiedBy(new SimpleCommand()));
        }
    }
}
