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

        public bool IsSatisfiedBy(object message)
        {
            Guard.AgainstNull(message, "message");

            return message.GetType().FullName.ToLower().StartsWith(startWith);
        }
    }
}