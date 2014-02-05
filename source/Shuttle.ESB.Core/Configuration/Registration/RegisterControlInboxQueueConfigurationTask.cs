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
                    WorkQueue = configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.ControlInbox.WorkQueueUri),
                    ErrorQueue = configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.ControlInbox.ErrorQueueUri),
                    ThreadCount = ServiceBusConfiguration.ServiceBusSection.ControlInbox.ThreadCount,
                    MaximumFailureCount = ServiceBusConfiguration.ServiceBusSection.ControlInbox.MaximumFailureCount,
                    DurationToIgnoreOnFailure = DurationToIgnoreOnFailure(ServiceBusConfiguration.ServiceBusSection.ControlInbox.DurationToIgnoreOnFailure),
                    DurationToSleepWhenIdle = DurationToSleepWhenIdle(ServiceBusConfiguration.ServiceBusSection.ControlInbox.DurationToSleepWhenIdle)
                };
        }
    }
}