using System.Configuration;

namespace Shuttle.ESB.Core
{
    public class WorkerElement : ConfigurationElement
    {
        public static readonly int ThreadAvailableNotificationIntervalSecondsDefault = 15;

        private static readonly ConfigurationProperty distributorControlWorkQueueUri =
            new ConfigurationProperty("distributorControlWorkQueueUri", typeof (string), string.Empty,
                                      ConfigurationPropertyOptions.IsRequired);

        private static readonly ConfigurationProperty threadAvailableNotificationIntervalSeconds =
            new ConfigurationProperty("threadAvailableNotificationIntervalSeconds", typeof(int), ThreadAvailableNotificationIntervalSecondsDefault,
                                      ConfigurationPropertyOptions.None);

        public WorkerElement()
        {
            base.Properties.Add(distributorControlWorkQueueUri);
            base.Properties.Add(threadAvailableNotificationIntervalSeconds);
        }

        [ConfigurationProperty("distributorControlWorkQueueUri", IsRequired = true)]
        public string DistributorControlWorkQueueUri
        {
            get { return (string)this[distributorControlWorkQueueUri]; }
        }

        [ConfigurationProperty("threadAvailableNotificationIntervalSeconds", IsRequired = false)]
        public int ThreadAvailableNotificationIntervalSeconds
        {
            get { return (int)this[threadAvailableNotificationIntervalSeconds]; }
        }
    }
}