using System.Configuration;

namespace Shuttle.ESB.Msmq
{
	public class MsmqSection : ConfigurationSection
	{
		public static MsmqSection Open(string file)
		{
			return ConfigurationManager
					   .OpenMappedMachineConfiguration(new ConfigurationFileMap(file))
					   .GetSection("msmq") as MsmqSection;
		}

		private static readonly ConfigurationProperty localQueueTimeoutMilliseconds =
			new ConfigurationProperty("localQueueTimeoutMilliseconds", typeof(int), 0, ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty remoteQueueTimeoutMilliseconds =
			new ConfigurationProperty("remoteQueueTimeoutMilliseconds", typeof(int), 2000, ConfigurationPropertyOptions.None);

		private static readonly ConfigurationProperty queues =
			new ConfigurationProperty("queues", typeof(MsmqQueueElementCollection), null,
									  ConfigurationPropertyOptions.None);

        public MsmqSection()
        {
			base.Properties.Add(localQueueTimeoutMilliseconds);
			base.Properties.Add(remoteQueueTimeoutMilliseconds);
			base.Properties.Add(queues);
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

		[ConfigurationProperty("queues", IsRequired = true)]
		public MsmqQueueElementCollection Queues
        {
			get { return (MsmqQueueElementCollection)this[queues]; }
        }
	}
}