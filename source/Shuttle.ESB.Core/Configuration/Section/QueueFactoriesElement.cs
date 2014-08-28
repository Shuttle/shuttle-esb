using System;
using System.Configuration;

namespace Shuttle.ESB.Core
{
	public class QueueFactoriesElement : ConfigurationElementCollection
    {
        [ConfigurationProperty("scan", IsRequired = false, DefaultValue = true)]
        public bool Scan
        {
            get
            {
				return (bool)this["scan"];
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
			return new QueueFactoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return Guid.NewGuid();
        }
    }
}