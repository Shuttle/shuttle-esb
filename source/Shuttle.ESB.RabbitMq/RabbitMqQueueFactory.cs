using System;
using System.Collections.Generic;
using System.Linq;
using RabbitMQ.Client;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMq
{
	public class RabbitMqQueueFactory : IQueueFactory, IDisposable
	{
		private readonly Dictionary<Uri, RabbitMqConnector> _connectors = new Dictionary<Uri, RabbitMqConnector>();		
		private readonly object _padlock = new object();
		private bool _disposed;

		internal const string SCHEME = "rabbitmq";
		internal const string INTERNAL_SCHEME = "amqp";

		public string Scheme { get { return SCHEME; } }

		public IQueue Create(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");
			return ConstructQueue(uri);
		}

		public bool CanCreate(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");
			return Scheme.Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase);
		}

		private IQueue ConstructQueue(Uri uri)
		{
			RabbitMqConnector connector = null;
			var queuePath = new RabbitMqQueuePath(uri);
			
			lock (_padlock)
			{				
				if (!_connectors.TryGetValue(uri, out connector))
				{
					connector = new RabbitMqConnector(queuePath.ConnnectUri);
					_connectors.Add(uri, connector);
				}
			}
						
			var channel = connector.Connection.CreateModel();
			if (!string.IsNullOrEmpty(queuePath.Exchange))
				channel.ExchangeDeclare(queuePath.Exchange, ExchangeType.Direct, true);
			return new RabbitMqQueue(channel, queuePath, true);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) return;

			if (disposing)
			{
				_connectors.ForEach(connector => connector.Value.Connection.Close());
				_connectors.Clear();
			}
			_disposed = true;
		}
	}
}