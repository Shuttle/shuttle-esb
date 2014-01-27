using System.Configuration;

namespace Shuttle.ESB.Msmq
{
	public class RabbitMQSection : ConfigurationSection
	{
		public static RabbitMQSection Open(string file)
		{
			return ConfigurationManager
					   .OpenMappedMachineConfiguration(new ConfigurationFileMap(file))
					   .GetSection("rabbitmq") as RabbitMQSection;
		}

		private static readonly ConfigurationProperty requestedHeartbeat =
			new ConfigurationProperty("requestedHeartbeat", typeof(ushort), 30, ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty localQueueTimeoutMilliseconds =
			new ConfigurationProperty("localQueueTimeoutMilliseconds", typeof(int), 0, ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty remoteQueueTimeoutMilliseconds =
			new ConfigurationProperty("remoteQueueTimeoutMilliseconds", typeof(int), 2000, ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty connectionCloseTimeoutMilliseconds =
			new ConfigurationProperty("connectionCloseTimeoutMilliseconds", typeof(int), 1000, ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty operationRetryCount =
			new ConfigurationProperty("operationRetryCount", typeof(int), 3, ConfigurationPropertyOptions.None);

        public RabbitMQSection()
        {
			base.Properties.Add(requestedHeartbeat);
			base.Properties.Add(localQueueTimeoutMilliseconds);
			base.Properties.Add(remoteQueueTimeoutMilliseconds);
			base.Properties.Add(connectionCloseTimeoutMilliseconds);
			base.Properties.Add(operationRetryCount);
        }

		[ConfigurationProperty("requestedHeartbeat", IsRequired = false)]
		public ushort RequestedHeartbeat
		{
			get
			{
				return (ushort)this[requestedHeartbeat];
			}
		}

		[ConfigurationProperty("localQueueTimeoutMilliseconds", IsRequired = false)]
		public int LocalQueueTimeoutMilliseconds
		{
			get
			{
				return (int)this[localQueueTimeoutMilliseconds];
			}
		}

		[ConfigurationProperty("remoteQueueTimeoutMilliseconds", IsRequired = false)]
		public int RemoteQueueTimeoutMilliseconds
		{
			get
			{
				return (int)this[remoteQueueTimeoutMilliseconds];
			}
		}

		[ConfigurationProperty("connectionCloseTimeoutMilliseconds", IsRequired = false)]
		public int ConnectionCloseTimeoutMilliseconds
		{
			get
			{
				return (int)this[connectionCloseTimeoutMilliseconds];
			}
		}

		[ConfigurationProperty("operationRetryCount", IsRequired = false)]
		public int OperationRetryCount
		{
			get
			{
				return (int)this[operationRetryCount];
			}
		}
	}
}