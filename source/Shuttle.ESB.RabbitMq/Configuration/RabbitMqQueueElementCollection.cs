using System.Configuration;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMq
{
	[ConfigurationCollection(typeof(MessageRouteElement), AddItemName = "queue", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	public class RabbitMqQueueElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new RabbitMqQueueElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((RabbitMqQueueElement)element).Uri;
		}
	}
}