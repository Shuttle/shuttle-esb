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

		public void Add(RabbitMqExchangeElement exchangeElement)
		{
			BaseAdd(exchangeElement);
		}

		public void Remove(RabbitMqExchangeElement exchangeElement)
		{
			if (BaseGet(exchangeElement.Name) == null)
				BaseRemove(exchangeElement.Name);
		}

		public void Clear()
		{
			BaseClear();
		}

		public RabbitMqExchangeElement GetLastAdded
		{
			get
			{
				return (RabbitMqExchangeElement) (Count == 0 ? null : BaseGet(Count - 1));
			}
		}
	}
}