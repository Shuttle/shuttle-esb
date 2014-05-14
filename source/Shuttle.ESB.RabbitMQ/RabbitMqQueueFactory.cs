using System;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMQ
{
	public class RabbitMQQueueFactory : IQueueFactory
	{
		public IRabbitMQConfiguration Configuration { get; private set; }

		public RabbitMQQueueFactory()
			: this(RabbitMQConfiguration.Default())
		{
		}

		public RabbitMQQueueFactory(IRabbitMQConfiguration configuration)
		{
			Configuration = configuration;
		}

		public string Scheme
		{
			get { return RabbitMQUriParser.SCHEME; }
		}

		public IQueue Create(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			return new RabbitMQQueue(uri, Configuration);
		}

		public bool CanCreate(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			return Scheme.Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}