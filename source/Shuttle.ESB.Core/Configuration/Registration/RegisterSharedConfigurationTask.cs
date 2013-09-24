namespace Shuttle.ESB.Core
{
    public class RegisterSharedConfigurationTask : RegistrationTask
    {
        public override void Execute(ServiceBusConfiguration configuration)
        {
            if (ServiceBusConfiguration.ServiceBusSection == null)
            {
                configuration.RemoveMessagesNotHandled = false;

                return;
            }

            configuration.RemoveMessagesNotHandled = ServiceBusConfiguration.ServiceBusSection.RemoveMessagesNotHandled;
            configuration.CompressionAlgorithm = ServiceBusConfiguration.ServiceBusSection.CompressionAlgorithm;
            configuration.EncryptionAlgorithm = ServiceBusConfiguration.ServiceBusSection.EncryptionAlgorithm;
        }
    }
}