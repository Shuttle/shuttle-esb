using System.Configuration;

namespace Shuttle.ESB.Core
{
    public class UriResolverItemElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
				return (string)this["name"];
            }
        }

		[ConfigurationProperty("uri", IsRequired = true)]
        public string Uri
        {
            get
            {
				return (string)this["uri"];
            }
        }
    }
}