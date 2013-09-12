using Shuttle.Management.Shell;

namespace Shuttle.Management.Subscriptions
{
	public interface ISubscriptionManagementPresenter : IManagementModulePresenter
	{
		void CheckAllSubscriptions();
		void InvertSubscriptionChecks();
		void RefreshSubscriptions();
		void RefreshSubscribers();
		void RemoveSubscriptions();
		void DataStoreChanged();
		void RefreshDataStores();
		void AddSubscriptions();
		void CheckAllEventMessageTypes();
		void InvertEventMessageTypeChecks();
	    void GetAssemblyEventMessageTypes();
	}
}