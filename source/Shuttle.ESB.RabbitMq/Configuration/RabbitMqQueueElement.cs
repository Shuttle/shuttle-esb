using System.Configuration;

namespace Shuttle.ESB.RabbitMq
{
	public class RabbitMqQueueElement : ConfigurationElement
	{
		private static readonly ConfigurationProperty _uri =
				new ConfigurationProperty("uri", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired);

		private static readonly ConfigurationProperty _isTransactional =
			new ConfigurationProperty("isTransactional", typeof(bool), true, ConfigurationPropertyOptions.IsRequired);

		private static readonly ConfigurationProperty _isDurable =
			new ConfigurationProperty("isDurable", typeof(bool), true, ConfigurationPropertyOptions.IsRequired);

		private static readonly ConfigurationProperty _autoDelete =
			new ConfigurationProperty("autoDelete", typeof(bool), false, ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty _isExclusive =
			new ConfigurationProperty("isExclusive", typeof(bool), false, ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty _exchange =
			new ConfigurationProperty("exchange", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty _routingKey =
			new ConfigurationProperty("routingKey", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty _overwriteIfExists =
			new ConfigurationProperty("overwriteIfExists", typeof(bool), false, ConfigurationPropertyOptions.None);


		public RabbitMqQueueElement()
		{
			base.Properties.Add(_uri);
			base.Properties.Add(_isTransactional);
			base.Properties.Add(_isDurable);
			base.Properties.Add(_autoDelete);
			base.Properties.Add(_isExclusive);
			base.Properties.Add(_exchange);
			base.Properties.Add(_routingKey);
			base.Properties.Add(_overwriteIfExists);
		}

		[ConfigurationProperty("uri", IsRequired = true)]
		public string Uri
		{
			get
			{
				return (string)this[_uri];
			}
			set
			{
				this[_uri] = value;
			}
		}

		[ConfigurationProperty("isTransactional", IsRequired = true)]
		public bool IsTransactional
		{
			get
			{
				return (bool)this[_isTransactional];
			}
			set
			{
				this[_isTransactional] = value;
			}
		}

		[ConfigurationProperty("isDurable", IsRequired = true)]
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

		[ConfigurationProperty("isExclusive", IsRequired = false)]
		public bool IsExclusive
		{
			get
			{
				return (bool)this[_isExclusive];
			}
			set
			{
				this[_isExclusive] = value;
			}
		}

		[ConfigurationProperty("exchange",  IsRequired = false)]
		public string Exchange
		{
			get
			{
				return (string)this[_exchange];
			}
			set
			{
				this[_exchange] = value;
			}
		}

		[ConfigurationProperty("routingKey", IsRequired = false)]
		public string RoutingKey
		{
			get
			{
				return (string)this[_routingKey];
			}
			set
			{
				this[_routingKey] = value;
			}
		}

		[ConfigurationProperty("overwriteIfExists", IsRequired = false)]
		public bool OverwriteIfExists
		{
			get
			{
				return (bool)this[_overwriteIfExists];
			}
			set
			{
				this[_overwriteIfExists] = value;
			}
		}
	}
}