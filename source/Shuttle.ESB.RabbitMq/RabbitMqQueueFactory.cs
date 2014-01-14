using System;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMQ
{
	public class RabbitMQQueueFactory : IQueueFactory
	{
		public string Scheme
		{
			get { return RabbitMQQueue.SCHEME; }
		}

		public IQueue Create(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			return new RabbitMQQueue(uri);
		}

		public bool CanCreate(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			return Scheme.Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}