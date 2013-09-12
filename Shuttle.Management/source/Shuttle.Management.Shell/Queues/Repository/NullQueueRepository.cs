using System;
using System.Collections.Generic;

namespace Shuttle.Management.Shell
{
    public class NullQueueRepository : IQueueRepository
    {
        public IEnumerable<Queue> All()
        {
            return new List<Queue>();
        }

        public void Save(Queue queue)
        {
            throw new NotImplementedException();
        }

    	public void Remove(string uri)
        {
            throw new NotImplementedException();
        }

    	public bool Contains(string uri)
    	{
    		return false;
    	}

    	public Queue Get(string uri)
    	{
    		return null;
    	}
    }
}