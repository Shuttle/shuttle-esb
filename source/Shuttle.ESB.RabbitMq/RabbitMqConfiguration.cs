using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.RabbitMq.Interfaces;

namespace Shuttle.ESB.RabbitMq
{
	public class RabbitMqConfiguration : IRabbitMqConfiguration, IRabbitMqExchangeConfiguration, IRabbitMqQueueConfiguration
	{
		//private readonly List<RabbitMqQueueConfiguration> _queueConfigurations = new List<RabbitMqQueueConfiguration>();

		private static RabbitMqSection _rabbitMqSection;

		public static RabbitMqSection RabbitMqSection
		{
			get
			{
				return _rabbitMqSection ??
				       (_rabbitMqSection = ConfigurationManager.GetSection("rabbitmq") as RabbitMqSection) ??
				       (_rabbitMqSection = new RabbitMqSection());
			}
		}

	
		public RabbitMqQueueElement FindQueueConfiguration(Uri uri)
		{
			foreach (RabbitMqQueueElement queue in _rabbitMqSection.Queues)
			{
				if (queue.Uri.Equals(uri.ToString(), StringComparison.InvariantCultureIgnoreCase))
					return queue;
			}
			return null;
		}

		public RabbitMqExchangeElement FindExchangeConfiguration(string name)
		{
			return (from RabbitMqExchangeElement exchangeConfig in _rabbitMqSection.Exchanges
				where exchangeConfig.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
				select exchangeConfig).SingleOrDefault();
		}

		public void RemoveAll()
		{
			foreach (RabbitMqQueueElement queue in _rabbitMqSection.Queues)
			{
				RemoveQueueConfiguration(new Uri(queue.Uri));
			}

			foreach (RabbitMqExchangeElement exchange in _rabbitMqSection.Exchanges)
			{
				RemoveExchangeConfiguration(exchange.Name);
			}
		}

		public static RabbitMqConfiguration Default()
		{
			var configuration = new RabbitMqConfiguration();

			if (RabbitMqSection != null && _rabbitMqSection.Queues != null)
			{
				foreach (RabbitMqQueueElement queue in _rabbitMqSection.Queues)
				{
					configuration
						.DeclareQueue(queue.Uri)
						.IsDurable(queue.IsDurable)
						.IsTransactional(queue.IsTransactional)
						.UseExchange(queue.Exchange)
						.RoutingKey(queue.RoutingKey)
						.AutoDelete(queue.AutoDelete)
						.IsExclusive(queue.IsExclusive);
				}
			}

			return configuration;
		}

		public void RemoveExchangeConfiguration(string name)
		{
			_rabbitMqSection.Exchanges.Remove(FindExchangeConfiguration(name));

			if (OnRemoveExchangeConfiguration != null)
			{
				OnRemoveExchangeConfiguration(name);
			}
		}

		public void RemoveQueueConfiguration(Uri uri)
		{
			_rabbitMqSection.Queues.Remove(FindQueueConfiguration(uri));

			if (OnRemoveQueueConfiguration != null)
			{
				OnRemoveQueueConfiguration(uri);
			}
		}

		#region Events

		public event Action<Uri> OnRemoveQueueConfiguration;

		public event Action<string> OnRemoveExchangeConfiguration;

		#endregion

		#region Fluent Exhange

		public IRabbitMqExchangeConfiguration DefineExchange(string name)
		{
			RabbitMqSection.Exchanges.Add(new RabbitMqExchangeElement() { Name = name });
			return this;
		}

		public IRabbitMqExchangeConfiguration Type(string type)
		{
			RabbitMqSection.Exchanges.GetLastAdded.Type = type;
			return this;
		}

		IRabbitMqExchangeConfiguration IRabbitMqExchangeConfiguration.IsDurable(bool value)
		{
			RabbitMqSection.Exchanges.GetLastAdded.IsDurable = value;
			return this;
		}

		#endregion

		#region Fluent Queue

		public IRabbitMqQueueConfiguration DeclareQueue(Uri uri)
		{
			_rabbitMqSection.Queues.Add(new RabbitMqQueueElement {Uri = uri.ToString()});
			return this;
		}

		public IRabbitMqQueueConfiguration DeclareQueue(string uri)
		{
			_rabbitMqSection.Queues.Add(new RabbitMqQueueElement { Uri = uri });
			return this;
		}

		public IRabbitMqQueueConfiguration IsDurable(bool value)
		{
			_rabbitMqSection.Queues.GetLastAdded.IsDurable = value;
			return this;
		}

		public IRabbitMqQueueConfiguration IsTransactional(bool value)
		{
			_rabbitMqSection.Queues.GetLastAdded.IsTransactional = value;
			return this;
		}

		public IRabbitMqQueueConfiguration UseExchange(string name)
		{
			_rabbitMqSection.Queues.GetLastAdded.Exchange = name;
			return this;
		}

		public IRabbitMqQueueConfiguration RoutingKey(string routePattern)
		{
			_rabbitMqSection.Queues.GetLastAdded.RoutingKey = routePattern;
			return this;
		}

		public IRabbitMqQueueConfiguration AutoDelete(bool value)
		{
			_rabbitMqSection.Queues.GetLastAdded.AutoDelete = value;
			return this;
		}

		public IRabbitMqQueueConfiguration IsExclusive(bool value)
		{
			_rabbitMqSection.Queues.GetLastAdded.IsExclusive = value;
			return this;
		}

		public IRabbitMqQueueConfiguration OverwriteIfExists
		{
			get	
			{
				_rabbitMqSection.Queues.GetLastAdded.OverwriteIfExists = true;
				return this;
			}
		}

		#endregion
	
	}
}