namespace Shuttle.ESB.Core
{
    public class WorkerConfiguration : IWorkerConfiguration
    {
        public WorkerConfiguration(IQueue distributorControlInboxWorkQueue, int threadAvailableNotificationIntervalSeconds)
        {
            DistributorControlInboxWorkQueue = distributorControlInboxWorkQueue;
            ThreadAvailableNotificationIntervalSeconds = threadAvailableNotificationIntervalSeconds;
        }

        public IQueue DistributorControlInboxWorkQueue { get; private set; }
        public int ThreadAvailableNotificationIntervalSeconds { get; private set; }
    }
}