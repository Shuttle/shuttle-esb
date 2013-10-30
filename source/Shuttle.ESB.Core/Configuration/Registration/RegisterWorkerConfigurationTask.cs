namespace Shuttle.ESB.Core
{
    public class RegisterWorkerConfigurationTask : RegistrationTask
    {
        public override void Execute(ServiceBusConfiguration configuration)
        {
            if (ServiceBusConfiguration.ServiceBusSection == null
                ||
                ServiceBusConfiguration.ServiceBusSection.Worker == null
                ||
                string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.Worker.DistributorControlWorkQueueUri))
            {
                return;
            }

            configuration.Worker =
                new WorkerConfiguration(QueueManager.Instance.CreateQueue(
                        ServiceBusConfiguration.ServiceBusSection.Worker.DistributorControlWorkQueueUri),
                        ServiceBusConfiguration.ServiceBusSection.Worker.ThreadAvailableNotificationIntervalSeconds);
        }
    }
}