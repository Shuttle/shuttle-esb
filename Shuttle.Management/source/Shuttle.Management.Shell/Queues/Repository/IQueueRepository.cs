using System.Collections.Generic;

namespace Shuttle.Management.Shell
{
    public interface IQueueRepository
    {
        IEnumerable<Queue> All();
        void Save(Queue queue);
    	void Remove(string uri);
    	bool Contains(string uri);
    	Queue Get(string uri);
    }
}