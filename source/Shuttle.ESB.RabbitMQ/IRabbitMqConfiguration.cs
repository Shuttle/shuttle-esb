namespace Shuttle.ESB.RabbitMQ
{
	public interface IRabbitMQConfiguration
	{
		ushort RequestedHeartbeat { get; set; }
		int LocalQueueTimeoutMilliseconds { get; set; }
		int RemoteQueueTimeoutMilliseconds { get; set; }
		int ConnectionCloseTimeoutMilliseconds { get; set; }
		int OperationRetryCount { get; set;  }
		int DefaultPrefetchCount { get; set;  }
	}
}