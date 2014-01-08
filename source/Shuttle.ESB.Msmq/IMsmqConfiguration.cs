using System;

namespace Shuttle.ESB.Msmq
{
	public interface IMsmqConfiguration
	{
		int LocalQueueTimeoutMilliseconds { get; set; }
		int RemoteQueueTimeoutMilliseconds { get; set; }
		void AddQueueConfiguration(MsmqQueueConfiguration queueConfiguration);
		MsmqQueueConfiguration FindQueueConfiguration(Uri uri);
		void RemoveQueueConfiguration(Uri uri);
	}
}