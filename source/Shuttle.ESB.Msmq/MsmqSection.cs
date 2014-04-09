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

		[ConfigurationProperty("localQueueTimeoutMilliseconds", IsRequired = false, DefaultValue = 0)]
		public int LocalQueueTimeoutMilliseconds
		{
			get
			{
				return (int)this["localQueueTimeoutMilliseconds"];
			}
		}

		[ConfigurationProperty("remoteQueueTimeoutMilliseconds", IsRequired = false, DefaultValue = 2000)]
		public int RemoteQueueTimeoutMilliseconds
		{
			get
			{
				return (int)this["remoteQueueTimeoutMilliseconds"];
			}
		}
	}
}