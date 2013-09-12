using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
    public interface IMessageRoute
    {
        IQueue Queue { get; }
        IMessageRoute AddSpecification(IMessageRouteSpecification specification);
        bool IsSatisfiedBy(object message);

        IEnumerable<IMessageRouteSpecification> Specifications { get; }
    }
}