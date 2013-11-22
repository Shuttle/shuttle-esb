using System.Configuration;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMq
{
	[ConfigurationCollection(typeof(MessageRouteElement), AddItemName = "exchange", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	public class RabbitMqExchangeElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new RabbitMqExchangeElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((RabbitMqExchangeElement)element).Name;
		}
	}
}