namespace Shuttle.ESB.Core
{
    public interface IErrorQueueConfiguration
    {
        IQueue ErrorQueue { get; }
    }
}