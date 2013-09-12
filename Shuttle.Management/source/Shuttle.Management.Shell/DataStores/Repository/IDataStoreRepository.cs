using System.Collections.Generic;

namespace Shuttle.Management.Shell
{
    public interface IDataStoreRepository
    {
        IEnumerable<DataStore> All();
        void Save(DataStore store);
    	void Remove(string name);
    	bool Contains(string name);
    	DataStore Get(string name);
    }
}