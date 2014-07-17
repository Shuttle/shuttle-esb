using System.Configuration;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Msmq
{
	public class MsmqSection : ConfigurationSection
	{
		public static MsmqSection Open(string file)
		{
			return ShuttleConfigurationSection.Open<MsmqSection>("msmq", file);
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