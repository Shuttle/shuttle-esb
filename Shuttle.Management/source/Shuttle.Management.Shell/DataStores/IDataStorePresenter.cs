namespace Shuttle.Management.Shell
{
	public interface IDataStorePresenter : IManagementModulePresenter
	{
        void Remove();
        void CheckAll();
        void InvertChecks();
        void Save();
        void Refresh();
	    void DataStoreSelected();
	}
}