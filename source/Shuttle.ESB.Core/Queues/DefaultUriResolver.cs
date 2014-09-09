using System;
using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
	public class DefaultUriResolver : IUriResolver
	{
		private readonly Dictionary<string, Uri> _uris = new Dictionary<string, Uri>();

		public DefaultUriResolver()
		{
			if (ServiceBusConfiguration.ServiceBusSection == null ||
			    ServiceBusConfiguration.ServiceBusSection.UriResolver == null)
			{
				return;
			}

			foreach (UriResolverItemElement uriRepositoryItemElement in ServiceBusConfiguration.ServiceBusSection.UriResolver)
			{
				_uris.Add(uriRepositoryItemElement.Name.ToLower(), new Uri(uriRepositoryItemElement.Uri)); 
			}
		}

		public Uri Get(string name)
		{
			var key = name.ToLower();

			return _uris.ContainsKey(key) ? _uris[key] : null;
		}
	}
}