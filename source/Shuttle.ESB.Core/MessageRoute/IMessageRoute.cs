using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
    public interface IMessageRoute
    {
        IQueue Queue { get; }
        IMessageRoute AddSpecification(IMessageRouteSpecification specification);
        bool IsSatisfiedBy(string messageType);

        IEnumerable<IMessageRouteSpecification> Specifications { get; }
    }
}