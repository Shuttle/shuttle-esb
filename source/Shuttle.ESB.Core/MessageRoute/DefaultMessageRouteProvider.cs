using System.Collections.Generic;
using System.Linq;

namespace Shuttle.ESB.Core
{
	public class DefaultMessageRouteProvider : IMessageRouteProvider, IRequireInitialization
	{
		private readonly IMessageRouteCollection messageRoutes = new MessageRouteCollection();

		public IEnumerable<string> GetRouteUris(object message)
		{
			var uri = messageRoutes.FindAll(message).Select(messageRoute => messageRoute.Queue.Uri.ToString()).FirstOrDefault();

			return
				string.IsNullOrEmpty(uri)
					? new List<string>()
					: new List<string> {uri};
		}

		public void Initialize(IServiceBus bus)
		{
			if (ServiceBusConfiguration.ServiceBusSection == null || ServiceBusConfiguration.ServiceBusSection.MessageRoutes == null)
			{
				return;
			}

			var factory = new MessageRouteSpecificationFactory();

			foreach (MessageRouteElement mapElement in ServiceBusConfiguration.ServiceBusSection.MessageRoutes)
			{
				var map = messageRoutes.Find(mapElement.Uri);

				if (map == null)
				{
					map = new MessageRoute(bus.Configuration.QueueManager.GetQueue(mapElement.Uri));

					messageRoutes.Add(map);
				}

				foreach (SpecificationElement specificationElement in mapElement)
				{
					map.AddSpecification(factory.Create(specificationElement.Name, specificationElement.Value));
				}
			}
		}
	}
}