using System.Text.RegularExpressions;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class RegexMessageRouteSpecification : IMessageRouteSpecification
    {
        private readonly Regex regex;

        public RegexMessageRouteSpecification(string pattern)
        {
            regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public bool IsSatisfiedBy(object message)
        {
            Guard.AgainstNull(message, "message");

            return regex.IsMatch(message.GetType().FullName);
        }
    }
}