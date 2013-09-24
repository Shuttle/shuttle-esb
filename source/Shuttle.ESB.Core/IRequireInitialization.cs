namespace Shuttle.ESB.Core
{
    public interface IRequireInitialization
    {
        void Initialize(IServiceBus bus);
    }
}