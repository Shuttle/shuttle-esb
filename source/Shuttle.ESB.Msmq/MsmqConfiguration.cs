using System;
using System.Collections.Generic;
using System.Configuration;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Msmq
{
	public class MsmqConfiguration : IMsmqConfiguration
	{
		private readonly List<MsmqQueueConfiguration> queueConfigurations = new List<MsmqQueueConfiguration>();

		private static MsmqSection msmqSection;

		public static MsmqSection MsmqSection
		{
			get
			{
				return msmqSection ??
					   (msmqSection = ConfigurationManager.GetSection("msmq") as MsmqSection);
			}
		}

		public int LocalQueueTimeoutMilliseconds { get; set; }
		public int RemoteQueueTimeoutMilliseconds { get; set; }

		public void AddQueueConfiguration(MsmqQueueConfiguration queueConfiguration)
		{
			Guard.AgainstNull(queueConfiguration, "queueConfiguration");

			queueConfigurations.Add(queueConfiguration);
		}

		public MsmqQueueConfiguration FindQueueConfiguration(Uri uri)
		{
			return queueConfigurations.Find(item => uri.Equals(item.Uri));
		}

		public static MsmqConfiguration Default()
		{
			var configuration = new MsmqConfiguration();

			if (MsmqSection != null && msmqSection.Queues != null)
			{
				configuration.LocalQueueTimeoutMilliseconds = MsmqSection.LocalQueueTimeoutMilliseconds;
				configuration.RemoteQueueTimeoutMilliseconds = MsmqSection.RemoteQueueTimeoutMilliseconds;

				foreach (MsmqQueueElement queue in msmqSection.Queues)
				{
					configuration.AddQueueConfiguration(new MsmqQueueConfiguration(new Uri(queue.Uri), queue.IsTransactional));
				}
			}

			return configuration;
		}

		public void RemoveQueueConfiguration(Uri uri)
		{
			queueConfigurations.Remove(FindQueueConfiguration(uri));
		}
	}
}