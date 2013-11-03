using System;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMq
{
	public class RabbitMqQueuePath
	{
		public RabbitMqQueuePath(Uri uri)
		{
			if (!uri.Scheme.Equals(RabbitMqQueueFactory.SCHEME, StringComparison.InvariantCultureIgnoreCase))
				throw new InvalidSchemeException(RabbitMqQueueFactory.SCHEME, uri.ToString());

			if (uri.LocalPath == "/")
				throw new UriFormatException(string.Format(ESBResources.UriFormatException,
																									 "rabbitmq://{{user:password@}}{{host-name}}/{{queue-name}}@{{exchange}}", uri));

			var builder = new UriBuilder(uri);

			if (uri.Host.Equals(".") ||
			    uri.Host.Equals("localhost", StringComparison.InvariantCultureIgnoreCase))
			{
				builder.Host = Environment.MachineName.ToLower();
			}

			Uri = builder.Uri;

			builder.Scheme = RabbitMqQueueFactory.INTERNAL_SCHEME;
			builder.Path = string.Empty;
			ConnnectUri = builder.Uri;
			Exchange = string.Empty;
			ExtractUriQueuePath();
		}

		public string Exchange { get; private set; }
		public string QueueName { get; private set; }
		public string Host { get { return Uri.Host; } }
		public Uri Uri { get; private set; }
		public Uri ConnnectUri { get; private set; }

		private void ExtractUriQueuePath()
		{			
			var split = Uri.PathAndQuery.Split('@');

			if (split.Length <= 2)
			{
				QueueName = split[0].Remove(0, 1);
			}

			if (split.Length == 2)
			{
				Exchange = split[1];
			}
		}
	}
}