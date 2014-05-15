using System;
using System.Configuration;

namespace Shuttle.ESB.Core
{
    public class MessageRouteElement : ConfigurationElementCollection
    {
        [ConfigurationProperty("uri", IsRequired = true)]
        public string Uri
        {
            get
            {
				return (string)this["uri"];
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SpecificationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return Guid.NewGuid();
        }
    }
}