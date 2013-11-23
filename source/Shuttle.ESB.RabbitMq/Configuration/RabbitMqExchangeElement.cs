using System.Configuration;
using RabbitMQ.Client;

namespace Shuttle.ESB.RabbitMq
{
	public class RabbitMqExchangeElement : ConfigurationElement
	{
		private static readonly ConfigurationProperty _name =
				new ConfigurationProperty("name", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired);

		private static readonly ConfigurationProperty _type=
			new ConfigurationProperty("type", typeof(string), ExchangeType.Direct, ConfigurationPropertyOptions.IsRequired);

		private static readonly ConfigurationProperty _isDurable =
			new ConfigurationProperty("isDurable", typeof(bool), true, ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty _autoDelete =
			new ConfigurationProperty("autoDelete", typeof(bool), false, ConfigurationPropertyOptions.None);

		public RabbitMqExchangeElement()
		{
			base.Properties.Add(_name);
			base.Properties.Add(_type);
			base.Properties.Add(_isDurable);
			base.Properties.Add(_autoDelete);
		}

		[ConfigurationProperty("name", IsRequired = true)]
		public string Name
		{
			get
			{
				return (string)this[_name];
			}
			set
			{
				this[_name] = value;
			}
		}

		[ConfigurationProperty("type", IsRequired = true)]
		public string Type
		{
			get
			{
				return (string)this[_type];
			}
			set
			{
				this[_type] = value;
			}
		}

		[ConfigurationProperty("isDurable", IsRequired = false)]
		public bool IsDurable
		{
			get
			{
				return (bool)this[_isDurable];
			}
			set
			{
				this[_isDurable] = value;
			}
		}

		[ConfigurationProperty("autoDelete", IsRequired = false)]
		public bool AutoDelete
		{
			get
			{
				return (bool)this[_autoDelete];
			}
			set
			{
				this[_autoDelete] = value;
			}
		}
	}
}