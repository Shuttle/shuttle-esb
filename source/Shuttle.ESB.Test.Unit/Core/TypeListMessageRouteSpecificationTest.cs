using NUnit.Framework;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Unit.Core
{
    [TestFixture]
    public class TypeListMessageRouteSpecificationTest : UnitFixture
    {
        [Test]
        public void Should_be_able_to_get_types_from_given_valid_value_string()
        {
            new TypeListMessageRouteSpecification(
                "Shuttle.ESB.Test.Shared.SimpleCommand, Shuttle.ESB.Test.Shared;" +
                "Shuttle.ESB.Test.Shared.SimpleMessage, Shuttle.ESB.Test.Shared;");
        }

        [Test]
        public void Should_fail_when_given_a_type_that_cannot_be_determined()
        {
            Assert.Throws<MessageRouteSpecificationException>(() => new TypeListMessageRouteSpecification("bogus"));
        }
    }
}