using System.Configuration;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Msmq
{
	[ConfigurationCollection(typeof(MessageRouteElement), AddItemName = "queue", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	public class MsmqQueueElementCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new MsmqQueueElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((MsmqQueueElement)element).Uri;
		}
	}
}