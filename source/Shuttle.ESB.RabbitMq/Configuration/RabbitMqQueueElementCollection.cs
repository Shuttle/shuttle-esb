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

		public void Add(RabbitMqQueueElement queueElement)
		{
			if (BaseGet(queueElement.Uri) == null)
				BaseAdd(queueElement);
		}

		public void Remove(RabbitMqQueueElement queueElement)
		{
			if (BaseGet(queueElement.Uri) == null)
				BaseRemove(queueElement.Uri);
		}
		
		public void Clear()
		{
			BaseClear();
		}

		public RabbitMqQueueElement GetLastAdded
		{
			get
			{
				return (RabbitMqQueueElement)(Count == 0 ? null : BaseGet(Count - 1));
			}
		}
	}
}