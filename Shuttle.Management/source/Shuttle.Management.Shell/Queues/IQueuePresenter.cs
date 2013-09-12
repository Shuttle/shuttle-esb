namespace Shuttle.Management.Shell
{
    public interface IQueuePresenter : IManagementModulePresenter
    {
    	void Remove();
    	void CheckAll();
    	void InvertChecks();
    	void Save();
    	void Refresh();
    	void QueueSelected();
    }
}
