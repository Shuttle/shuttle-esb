using System.Reflection;
using NUnit.Framework;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Unit.Core
{
    [TestFixture]
    public class AssemblyMessageRouteSpecificationTest : UnitFixture
    {
        [Test]
        public void Should_be_able_to_get_all_the_message_types_from_a_given_assembly()
        {
            new AssemblyMessageRouteSpecification(Assembly.GetExecutingAssembly());
        }

        [Test]
        public void Should_be_able_to_get_all_the_message_types_from_a_given_valid_assembly_name()
        {
            new AssemblyMessageRouteSpecification("Shuttle.ESB.Test.Unit");
            new AssemblyMessageRouteSpecification("Shuttle.ESB.Test.Shared");
        }

        [Test]
        public void Should_fail_when_given_an_invalid_assembly_name()
        {
            Assert.Throws<MessageRouteSpecificationException>(() => new AssemblyMessageRouteSpecification("bogus"));
        }
    }
}