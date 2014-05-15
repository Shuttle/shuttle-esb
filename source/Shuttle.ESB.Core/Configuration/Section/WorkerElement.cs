using System.Configuration;

namespace Shuttle.ESB.Core
{
    public class WorkerElement : ConfigurationElement
    {
        [ConfigurationProperty("distributorControlWorkQueueUri", IsRequired = true)]
        public string DistributorControlWorkQueueUri
        {
            get { return (string)this["distributorControlWorkQueueUri"]; }
        }

        [ConfigurationProperty("threadAvailableNotificationIntervalSeconds", IsRequired = false, DefaultValue = 15)]
        public int ThreadAvailableNotificationIntervalSeconds
        {
            get { return (int)this["threadAvailableNotificationIntervalSeconds"]; }
        }
    }
}