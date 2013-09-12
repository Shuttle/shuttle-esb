namespace Shuttle.Management.Scheduling
{
    public interface IScheduleManagementPresenter
    {
        void RefreshSchedules();
    	void MarkAllSchedules();
    	void InvertMarkedSchedules();
    	void RefreshDataStores();
    	void DataStoreChanged();
    	void OnViewReady();
    	void RemoveSchedule();
    	void CheckAllSchedules();
    	void InvertScheduleChecks();
    	void SaveSchedule();
    }
}