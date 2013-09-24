using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Shuttle.ESB.Core
{
    public class MessageRoute : IMessageRoute
    {
        private readonly List<IMessageRouteSpecification> specifications = new List<IMessageRouteSpecification>();

        public MessageRoute(IQueue queue)
        {
            Queue = queue;
        }

        public IQueue Queue { get; private set; }

        public IMessageRoute AddSpecification(IMessageRouteSpecification specification)
        {
            specifications.Add(specification);

            return this;
        }

        public bool IsSatisfiedBy(object message)
        {
            foreach (var specification in specifications)
            {
                if (specification.IsSatisfiedBy(message))
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<IMessageRouteSpecification> Specifications
        {
            get
            {
                return new ReadOnlyCollection<IMessageRouteSpecification>(specifications);
            }
        }
    }
}