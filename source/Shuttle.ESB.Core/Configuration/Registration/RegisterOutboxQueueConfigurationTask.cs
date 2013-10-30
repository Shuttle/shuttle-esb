namespace Shuttle.ESB.Core
{
    public class RegisterOutboxQueueConfigurationTask : RegistrationTask
    {
        public override void Execute(ServiceBusConfiguration configuration)
        {
            if (ServiceBusConfiguration.ServiceBusSection == null
                || 
                ServiceBusConfiguration.ServiceBusSection.Outbox == null 
                || 
                string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.Outbox.WorkQueueUri))
            {
                return;
            }

            configuration.Outbox =
                new OutboxQueueConfiguration
                {
                    WorkQueue = QueueManager.Instance.GetQueue(ServiceBusConfiguration.ServiceBusSection.Outbox.WorkQueueUri),
                    ErrorQueue = QueueManager.Instance.GetQueue(ServiceBusConfiguration.ServiceBusSection.Outbox.ErrorQueueUri),
                    MaximumFailureCount = ServiceBusConfiguration.ServiceBusSection.Outbox.MaximumFailureCount,
                    DurationToIgnoreOnFailure = DurationToIgnoreOnFailure(ServiceBusConfiguration.ServiceBusSection.Outbox.DurationToIgnoreOnFailure),
                    DurationToSleepWhenIdle = DurationToSleepWhenIdle(ServiceBusConfiguration.ServiceBusSection.Outbox.DurationToSleepWhenIdle),
					ThreadCount = ServiceBusConfiguration.ServiceBusSection.Inbox.ThreadCount
				};
        }
    }
}