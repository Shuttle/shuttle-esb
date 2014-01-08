using System;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMQ
{
	public class RabbitMQQueueFactory : IQueueFactory, IDisposable
	{
		private IRabbitMqManager _manager;

		public RabbitMQQueueFactory()
			: this(new RabbitMQManager())
		{
		}
		
		public RabbitMQQueueFactory(IRabbitMqManager manager)
		{
			_manager = manager;
		}

		public string Scheme
		{
			get { return RabbitMQQueue.SCHEME; }
		}

		public IQueue Create(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			return new RabbitMQQueue(uri, _manager);
		}

		public bool CanCreate(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			return Scheme.Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase);
		}

		public void Initialize(IServiceBus bus)
		{
			_manager = new RabbitMQManager();
		}

		public void Dispose()
		{
			_manager.AttemptDispose();
		}
	}
}