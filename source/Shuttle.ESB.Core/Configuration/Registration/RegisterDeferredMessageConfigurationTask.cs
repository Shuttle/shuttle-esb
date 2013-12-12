namespace Shuttle.ESB.Core
{
    public class RegisterDeferredMessageConfigurationTask : RegistrationTask
    {
        public override void Execute(ServiceBusConfiguration configuration)
        {
            if (ServiceBusConfiguration.ServiceBusSection == null
                ||
                ServiceBusConfiguration.ServiceBusSection.DeferredMessage == null)
            {
                return;
            }

	        configuration.DeferredMessageConfiguration.DurationToSleepWhenIdle =
		        ServiceBusConfiguration.ServiceBusSection.DeferredMessage.DurationToSleepWhenIdle;
        }
    }
}