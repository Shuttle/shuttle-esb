namespace Shuttle.ESB.Msmq
{
	public interface IMsmqConfiguration
	{
		int LocalQueueTimeoutMilliseconds { get; set; }
		int RemoteQueueTimeoutMilliseconds { get; set; }
	}
}