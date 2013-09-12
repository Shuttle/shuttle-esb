using Shuttle.Management.Shell;

namespace Shuttle.Management.Subscriptions
{
    public interface ISubscriptionRequestManagementPresenter : IManagementModulePresenter
    {
    	void AcceptRequests();
    	void DeclineRequests();
    	void CheckAll();
    	void InvertChecks();
    	void RefreshRequests();
    	void RefreshSubscribers();
    	void DataStoreChanged();
		void RefreshDataStores();
	}
}