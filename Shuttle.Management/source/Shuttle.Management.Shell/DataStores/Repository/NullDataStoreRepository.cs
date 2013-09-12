using System;
using System.Collections.Generic;

namespace Shuttle.Management.Shell
{
    public class NullDataStoreRepository : IDataStoreRepository
    {
        public IEnumerable<DataStore> All()
        {
            return new List<DataStore>();
        }

        public void Save(DataStore store)
        {
            throw new NotImplementedException();
        }

    	public void Remove(string name)
        {
            throw new NotImplementedException();
        }

    	public bool Contains(string name)
    	{
    		return false;
    	}

    	public DataStore Get(string name)
    	{
    		return null;
    	}
    }
}