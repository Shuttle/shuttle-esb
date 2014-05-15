using System.Configuration;

namespace Shuttle.ESB.Core
{
    public class SpecificationElement : ConfigurationElement
    {
        [ConfigurationProperty("specification", IsRequired = true)]
        public string Name
        {
            get
            {
				return (string)this["specification"];
            }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
				return (string)this["value"];
            }
        }
    }
}