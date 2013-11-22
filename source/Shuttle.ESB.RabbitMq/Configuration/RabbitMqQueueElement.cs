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


		public RabbitMqQueueElement()
		{
			base.Properties.Add(_uri);
			base.Properties.Add(_isTransactional);
			base.Properties.Add(_isDurable);
			base.Properties.Add(_autoDelete);
			base.Properties.Add(_isExclusive);
			base.Properties.Add(_exchange);
		}

		[ConfigurationProperty("uri", IsRequired = true)]
		public string Uri
		{
			get
			{
				return (string)this[_uri];
			}
		}

		[ConfigurationProperty("isTransactional", IsRequired = true)]
		public bool IsTransactional
		{
			get
			{
				return (bool)this[_isTransactional];
			}
		}

		[ConfigurationProperty("isDurable", IsRequired = true)]
		public bool IsDurable
		{
			get
			{
				return (bool)this[_isDurable];
			}
		}

		[ConfigurationProperty("autoDelete", IsRequired = false)]
		public bool AutoDelete
		{
			get
			{
				return (bool)this[_autoDelete];
			}
		}

		[ConfigurationProperty("isExclusive", IsRequired = false)]
		public bool IsExclusive
		{
			get
			{
				return (bool)this[_isExclusive];
			}
		}

		[ConfigurationProperty("exchange",  IsRequired = false)]
		public string Exchange
		{
			get
			{
				return (string)this[_exchange];
			}
		}
	}
}