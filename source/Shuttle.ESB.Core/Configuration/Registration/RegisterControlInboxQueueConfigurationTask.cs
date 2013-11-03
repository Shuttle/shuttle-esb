namespace Shuttle.ESB.Core
{
    public class RegisterControlInboxQueueConfigurationTask : RegistrationTask
    {
        public override void Execute(ServiceBusConfiguration configuration)
        {
            if (ServiceBusConfiguration.ServiceBusSection == null
                ||
                ServiceBusConfiguration.ServiceBusSection.ControlInbox == null 
                ||
                string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.ControlInbox.WorkQueueUri) 
                ||
                string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.ControlInbox.ErrorQueueUri))
            {
                return;
            }

            configuration.ControlInbox =
                new ControlInboxQueueConfiguration
                {
                    WorkQueue = QueueManager.Instance.GetQueue(ServiceBusConfiguration.ServiceBusSection.ControlInbox.WorkQueueUri),
                    ErrorQueue = QueueManager.Instance.GetQueue(ServiceBusConfiguration.ServiceBusSection.ControlInbox.ErrorQueueUri),
                    JournalQueue = string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.ControlInbox.JournalQueueUri)
                                       ? null
                                       : QueueManager.Instance.GetQueue(ServiceBusConfiguration.ServiceBusSection.ControlInbox.JournalQueueUri),
                    ThreadCount = ServiceBusConfiguration.ServiceBusSection.ControlInbox.ThreadCount,
                    MaximumFailureCount = ServiceBusConfiguration.ServiceBusSection.ControlInbox.MaximumFailureCount,
                    DurationToIgnoreOnFailure = DurationToIgnoreOnFailure(ServiceBusConfiguration.ServiceBusSection.ControlInbox.DurationToIgnoreOnFailure),
                    DurationToSleepWhenIdle = DurationToSleepWhenIdle(ServiceBusConfiguration.ServiceBusSection.ControlInbox.DurationToSleepWhenIdle)
                };
        }
    }
}