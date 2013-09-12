using System.Collections.Generic;

namespace Shuttle.Management.Shell
{
	public interface IQueueView 
    {
        string UriValue { get; set; }
		bool HasSelectedQueues { get; }
		void CheckAll();
		void InvertChecks();
		void RefreshQueues(IEnumerable<Queue> list);
		IEnumerable<Queue> SelectedQueues();
		string GetSelectedQueueUri();
    }
}
