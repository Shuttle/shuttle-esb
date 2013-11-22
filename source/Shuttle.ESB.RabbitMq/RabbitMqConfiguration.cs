using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.RabbitMq
{
	public class RabbitMqConfiguration
	{
		private readonly List<RabbitMqQueueConfiguration> _queueConfigurations = new List<RabbitMqQueueConfiguration>();

		private static RabbitMqSection _rabbitMqSection;

		public static RabbitMqSection RabbitMqSection
		{
			get
			{
				return _rabbitMqSection ??
						 (_rabbitMqSection = ConfigurationManager.GetSection("rabbitmq") as RabbitMqSection);
			}
		}

		public void AddQueueConfiguration(RabbitMqQueueConfiguration queueConfiguration)
		{
			Guard.AgainstNull(queueConfiguration, "queueConfiguration");

			_queueConfigurations.Add(queueConfiguration);
		}

		public RabbitMqQueueConfiguration FindQueueConfiguration(Uri uri)
		{
			return _queueConfigurations.Find(item => uri.Equals(item.Uri));
		}

		public RabbitMqExchangeElement FindExchangeConfiguration(string name)
		{
			return (from RabbitMqExchangeElement exchangeConfig in _rabbitMqSection.Exchanges
				where exchangeConfig.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
				select exchangeConfig).SingleOrDefault();
		}

		public static RabbitMqConfiguration Default()
		{
			var configuration = new RabbitMqConfiguration();

			if (RabbitMqSection != null && _rabbitMqSection.Queues != null)
			{
				foreach (RabbitMqQueueElement queue in _rabbitMqSection.Queues)
				{
					var queueConfiguration = new RabbitMqQueueConfiguration(
						new Uri(queue.Uri),
						queue.IsTransactional,
						queue.IsDurable,
						queue.Exchange,
						queue.AutoDelete,
						queue.IsExclusive);

					configuration.AddQueueConfiguration(queueConfiguration);
				}
			}

			return configuration;
		}

		public void RemoveQueueConfiguration(Uri uri)
		{
			_queueConfigurations.Remove(FindQueueConfiguration(uri));

			if (OnRemoveQueueConfiguration != null)
			{
				OnRemoveQueueConfiguration(uri);
			}
		}

		public event Action<Uri> OnRemoveQueueConfiguration;
	}
}