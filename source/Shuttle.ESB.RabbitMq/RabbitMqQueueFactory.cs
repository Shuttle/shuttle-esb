using System;
using System.Collections.Generic;
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
		lock (_padlock)
			{
				RabbitMqConnector connector = null;
				var queuePath = new RabbitMqQueuePath(uri);

				if (!_connectors.TryGetValue(uri, out connector))
				{
					connector = new RabbitMqConnector(queuePath);
					_connectors.Add(uri, connector);
				}			

				return new RabbitMqQueue(connector, queuePath, false);
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			if (disposing)
			{
				_connectors.ForEach(connector => connector.Value.Close());
				_connectors.Clear();
			}
			_disposed = true;
		}
	}
}