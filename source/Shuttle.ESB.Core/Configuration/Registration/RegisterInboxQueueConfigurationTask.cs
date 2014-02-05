namespace Shuttle.ESB.Core
{
    public class RegisterInboxQueueConfigurationTask : RegistrationTask
    {
        public override void Execute(ServiceBusConfiguration configuration)
        {
            if (ServiceBusConfiguration.ServiceBusSection == null
                ||
                ServiceBusConfiguration.ServiceBusSection.Inbox == null
                ||
                string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.Inbox.WorkQueueUri))
            {
                return;
            }

            configuration.Inbox =
                new InboxQueueConfiguration
                {
					WorkQueue = configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.Inbox.WorkQueueUri),
                    ErrorQueue = configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.Inbox.ErrorQueueUri),
                    WorkQueueStartupAction = ServiceBusConfiguration.ServiceBusSection.Inbox.WorkQueueStartupAction,
                    ThreadCount = ServiceBusConfiguration.ServiceBusSection.Inbox.ThreadCount,
                    MaximumFailureCount = ServiceBusConfiguration.ServiceBusSection.Inbox.MaximumFailureCount,
                    DurationToIgnoreOnFailure = DurationToIgnoreOnFailure(ServiceBusConfiguration.ServiceBusSection.Inbox.DurationToIgnoreOnFailure),
                    DurationToSleepWhenIdle = DurationToSleepWhenIdle(ServiceBusConfiguration.ServiceBusSection.Inbox.DurationToSleepWhenIdle),
                    Distribute = ServiceBusConfiguration.ServiceBusSection.Inbox.Distribute
                };
        }
    }
}