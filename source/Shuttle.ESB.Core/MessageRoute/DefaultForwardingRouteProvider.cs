using System.Collections.Generic;
using System.Linq;

namespace Shuttle.ESB.Core
{
	public class DefaultForwardingRouteProvider : IMessageRouteProvider, IRequireInitialization
	{
		private readonly IMessageRouteCollection messageRoutes = new MessageRouteCollection();

		public IEnumerable<string> GetRouteUris(object message)
		{
			return messageRoutes.FindAll(message).Select(messageRoute => messageRoute.Queue.Uri.ToString()).ToList();
		}

		public void Initialize(IServiceBus bus)
		{
			if (ServiceBusConfiguration.ServiceBusSection == null || ServiceBusConfiguration.ServiceBusSection.ForwardingRoutes == null)
			{
				return;
			}

			var factory = new MessageRouteSpecificationFactory();

			foreach (MessageRouteElement mapElement in ServiceBusConfiguration.ServiceBusSection.ForwardingRoutes)
			{
				var map = messageRoutes.Find(mapElement.Uri);

				if (map == null)
				{
					map = new MessageRoute(QueueManager.Instance.GetQueue(mapElement.Uri));

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