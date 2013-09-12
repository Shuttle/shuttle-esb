namespace Shuttle.Management.Shell
{
	public interface IManagementConfiguration
	{
		string QueueRepositoryType { get; }
	    bool HasQueueRepository { get; }
	    IQueueRepository QueueRepository();

		string DataStoreRepositoryType { get; }
	    bool HasDataStoreRepository { get; }
	    IDataStoreRepository DataStoreRepository();

		void Initialize();
	}
}