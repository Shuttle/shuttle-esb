namespace Shuttle.ESB.Core
{
    public interface IWorkerConfiguration
    {
        IQueue DistributorControlInboxWorkQueue { get; }
        int ThreadAvailableNotificationIntervalSeconds { get; }
    }
}