using System.Configuration;

namespace Shuttle.ESB.Core
{
    public class SpecificationElement : ConfigurationElement
    {
        private static readonly ConfigurationProperty specification =
            new ConfigurationProperty("specification", typeof (string), string.Empty, ConfigurationPropertyOptions.IsRequired);

        private static readonly ConfigurationProperty value =
            new ConfigurationProperty("value", typeof (string), string.Empty, ConfigurationPropertyOptions.IsRequired);

        public SpecificationElement()
        {
            base.Properties.Add(specification);
            base.Properties.Add(value);
        }

        [ConfigurationProperty("specification", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this[specification];
            }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return (string)this[value];
            }
        }
    }
}