using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.ESB.RabbitMq.Exceptions;
using Shuttle.ESB.RabbitMq.Interfaces;

namespace Shuttle.ESB.RabbitMq
{
	public class RabbitMqQueueFactory : IQueueFactory, IDisposable
	{
		private readonly Dictionary<Uri, RabbitMqConnector> _connectors = new Dictionary<Uri, RabbitMqConnector>();
		private readonly object _padlock = new object();
		private bool _disposed;

		internal const string SCHEME = "rabbitmq";
		internal const string INTERNAL_SCHEME = "amqp";

		public RabbitMqQueueFactory()
			: this(RabbitMqConfiguration.Default())
		{
		}

		public RabbitMqQueueFactory(IRabbitMqConfiguration configuration)
		{
			Configuration = (RabbitMqConfiguration)configuration;

			Configuration.OnRemoveQueueConfiguration += delegate(Uri uri)
			{
				lock (_padlock)
				{
					if (_connectors.ContainsKey(uri))
					{
						var connector = _connectors[uri];
						connector.Close();
						_connectors.Remove(uri);
					}
				}
			};

		}

		public RabbitMqConfiguration Configuration { get; private set; }

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

				var queueConfiguration = Configuration.FindQueueConfiguration(uri);

				if (queueConfiguration == null)
				{
					Configuration.DeclareQueue(uri);
					queueConfiguration = Configuration.FindQueueConfiguration(uri);
				}

				if (!_connectors.TryGetValue(uri, out connector))
				{
					RabbitMqExchangeElement exchangeConfiguration = null;

					if (!string.IsNullOrEmpty(queueConfiguration.Exchange))
					{
						exchangeConfiguration = Configuration.FindExchangeConfiguration(queueConfiguration.Exchange);
						if (exchangeConfiguration == null)
							throw new UndefinedExchangeException(string.Format(RabbitMqResources.UndefinedExchange, queueConfiguration.Exchange));
					}

					connector = new RabbitMqConnector(exchangeConfiguration, queueConfiguration, queuePath);
					_connectors.Add(uri, connector);
				}

				return new RabbitMqQueue(connector);
			}
		}

		public void Dispose()
		{
			lock (_padlock)
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}
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