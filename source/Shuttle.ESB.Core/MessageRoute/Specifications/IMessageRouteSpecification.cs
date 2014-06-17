namespace Shuttle.ESB.Core
{
    public interface IMessageRouteSpecification
    {
        bool IsSatisfiedBy(string messageType);
    }
}