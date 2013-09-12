using System.Linq;
using System.Text;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class FindMessageRouteObserver : IPipelineObserver<OnFindRouteForMessage>
	{
		public void Execute(OnFindRouteForMessage pipelineEvent)
		{
			var queueUri = pipelineEvent.GetDestinationQueue() != null
			               	? pipelineEvent.GetDestinationQueue().Uri.ToString()
			               	: FindRoute(pipelineEvent.GetServiceBus(), pipelineEvent.GetMessage());

			pipelineEvent.GetTransportMessage().RecipientInboxWorkQueueUri = queueUri;
		}

		private static string FindRoute(IServiceBus bus, IMessage message)
		{
			Guard.AgainstNullDependency(bus.Configuration.MessageRouteProvider, "MessageRouteProvider");

			var routeUris = bus.Configuration.MessageRouteProvider.GetRouteUris(message);

			if (routeUris.Count() <= 0)
			{
				throw new SendMessageException(string.Format(Resources.MessageRouteNotFound, message.GetType().FullName));
			}

			if (routeUris.Count() > 1)
			{
				var uris = new StringBuilder();

				foreach (var route in routeUris)
				{
					uris.AppendFormat("{0}{1}", uris.Length > 0
					                            	? ","
					                            	: string.Empty, route);
				}

				throw new SendMessageException(string.Format(Resources.MessageRoutedToMoreThanOneEndpoint,
				                                             message.GetType().FullName,
				                                             uris));
			}

			return routeUris.ElementAt(0);
		}
	}
}