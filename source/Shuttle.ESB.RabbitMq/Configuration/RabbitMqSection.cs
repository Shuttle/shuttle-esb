using System.Configuration;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMq
{
	public class RabbitMqSection : ConfigurationSection
	{
		public static RabbitMqSection Open(string file)
		{
			return ConfigurationManager
						 .OpenMappedMachineConfiguration(new ConfigurationFileMap(file))
						 .GetSection("rabbitmq") as RabbitMqSection;
		}

		private static readonly ConfigurationProperty _queues =
			new ConfigurationProperty("queues", typeof(RabbitMqQueueElementCollection), null,
										ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty _exchanges =
			new ConfigurationProperty("exchanges", typeof(RabbitMqExchangeElementCollection), null,
										ConfigurationPropertyOptions.None);

		public RabbitMqSection()
		{
			base.Properties.Add(_exchanges);
			base.Properties.Add(_queues);
		}

		[ConfigurationProperty("queues", IsRequired = true)]
		public RabbitMqQueueElementCollection Queues
		{
			get { return (RabbitMqQueueElementCollection)this[_queues]; }
		}

		[ConfigurationProperty("exchanges", IsRequired = true)]
		public RabbitMqExchangeElementCollection Exchanges
		{
			get { return (RabbitMqExchangeElementCollection)this[_exchanges]; }
		}
	}
}