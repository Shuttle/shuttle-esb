using System.Configuration;

namespace Shuttle.ESB.RabbitMQ
{
	public class RabbitMQConfiguration : IRabbitMQConfiguration
	{
		private static RabbitMQSection section;

		public RabbitMQConfiguration()
		{
			RequestedHeartbeat = 30;
			LocalQueueTimeoutMilliseconds = 250;
			RemoteQueueTimeoutMilliseconds = 1000;
			ConnectionCloseTimeoutMilliseconds = 1000;
			OperationRetryCount = 3;
			DefaultPrefetchCount = 25;
		}

		public static RabbitMQSection RabbitMQSection
		{
			get
			{
				return section ??
				       (section = ConfigurationManager.GetSection("RabbitMQ") as RabbitMQSection);
			}
		}

		public ushort RequestedHeartbeat { get; set; }
		public int LocalQueueTimeoutMilliseconds { get; set; }
		public int RemoteQueueTimeoutMilliseconds { get; set; }
		public int ConnectionCloseTimeoutMilliseconds { get; set; }
		public int OperationRetryCount { get; set; }
		public ushort DefaultPrefetchCount { get; set; }

		public static RabbitMQConfiguration Default()
		{
			var configuration = new RabbitMQConfiguration();

			if (RabbitMQSection != null)
			{
				configuration.RequestedHeartbeat = section.RequestedHeartbeat;
				configuration.LocalQueueTimeoutMilliseconds = section.LocalQueueTimeoutMilliseconds;
				configuration.RemoteQueueTimeoutMilliseconds = section.RemoteQueueTimeoutMilliseconds;
				configuration.ConnectionCloseTimeoutMilliseconds = section.ConnectionCloseTimeoutMilliseconds;
				configuration.OperationRetryCount = section.OperationRetryCount;
				configuration.DefaultPrefetchCount = section.DefaultPrefetchCount;
			}

			return configuration;
		}
	}
}