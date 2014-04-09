using System.Configuration;

namespace Shuttle.ESB.Msmq
{
	public class MsmqConfiguration : IMsmqConfiguration
	{
		private static MsmqSection section;

		public MsmqConfiguration()
		{
			LocalQueueTimeoutMilliseconds = 0;
			RemoteQueueTimeoutMilliseconds = 2000;
		}

		public static MsmqSection MsmqSection
		{
			get
			{
				return section ??
				       (section = ConfigurationManager.GetSection("msmq") as MsmqSection);
			}
		}

		public int LocalQueueTimeoutMilliseconds { get; set; }
		public int RemoteQueueTimeoutMilliseconds { get; set; }

		public static MsmqConfiguration Default()
		{
			var configuration = new MsmqConfiguration();

			if (MsmqSection != null)
			{
				configuration.LocalQueueTimeoutMilliseconds = MsmqSection.LocalQueueTimeoutMilliseconds;
				configuration.RemoteQueueTimeoutMilliseconds = MsmqSection.RemoteQueueTimeoutMilliseconds;
			}

			return configuration;
		}
	}
}