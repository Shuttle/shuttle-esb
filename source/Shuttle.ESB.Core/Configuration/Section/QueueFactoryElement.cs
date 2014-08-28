using System.Configuration;

namespace Shuttle.ESB.Core
{
    public class QueueFactoryElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
				return (string)this["type"];
            }
        }
    }
}