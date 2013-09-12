using System;
using System.Configuration;

namespace Shuttle.ESB.Core
{
    public class MessageRouteElement : ConfigurationElementCollection
    {
        private static readonly ConfigurationProperty uri =
            new ConfigurationProperty("uri", typeof (string), string.Empty, ConfigurationPropertyOptions.IsRequired);

        public MessageRouteElement()
        {
            base.Properties.Add(uri);
        }

        [ConfigurationProperty("uri", IsRequired = true)]
        public string Uri
        {
            get
            {
                return (string)this[uri];
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