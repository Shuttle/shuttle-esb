using System.Collections.Generic;

namespace Shuttle.Management.Shell
{
	public interface IDataStoreView
	{
		string NameValue { get; set; }
		string ConnectionStringValue { get; set; }
		string ProviderNameValue { get; set; }
		void AddProviderName(string providerName);

        bool HasSelectedDataStores { get; }
        void CheckAll();
        void InvertChecks();
        void RefreshDataStores(IEnumerable<DataStore> list);
        IEnumerable<DataStore> SelectedDataStores();
	    DataStore GetSelectedDataStore();
	}
}