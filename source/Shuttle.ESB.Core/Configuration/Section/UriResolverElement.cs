using System;
using System.Configuration;

namespace Shuttle.ESB.Core
{
	public class UriResolverElement : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
			return new UriResolverItemElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return Guid.NewGuid();
        }
    }
}