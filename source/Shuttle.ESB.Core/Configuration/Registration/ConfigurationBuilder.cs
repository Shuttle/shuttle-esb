using System.Collections.Generic;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    internal class ConfigurationBuilder
    {
        private ConfigurationBuilder()
        {
        }

        public static ServiceBusConfiguration Build()
        {
            return Build(new ServiceBusConfiguration());
        }

        public static ServiceBusConfiguration Build(ServiceBusConfiguration configuration)
        {
            Guard.AgainstNull(configuration, "configuration");

            var tasks = new List<RegistrationTask>
                    {
                        new RegisterSharedConfigurationTask(),
                        new RegisterControlInboxQueueConfigurationTask(),
                        new RegisterInboxQueueConfigurationTask(),
                        new RegisterOutboxQueueConfigurationTask(),
                        new RegisterWorkerConfigurationTask()
                    };

            foreach (var task in tasks)
            {
                task.Execute(configuration);
            }

            return configuration;
        }
    }
}