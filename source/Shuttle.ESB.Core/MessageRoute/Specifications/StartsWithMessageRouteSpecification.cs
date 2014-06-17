using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class StartsWithMessageRouteSpecification : IMessageRouteSpecification
    {
        private readonly string startWith;

        public StartsWithMessageRouteSpecification(string startWith)
        {
            this.startWith = startWith.ToLower();
        }

        public bool IsSatisfiedBy(string messageType)
        {
            Guard.AgainstNull(messageType, "message");

            return messageType.ToLower().StartsWith(startWith);
        }
    }
}