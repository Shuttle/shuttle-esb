using System;
using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
    public interface IQueueManager
    {
        IQueueFactory GetQueueFactory(string uri);
        IQueueFactory GetQueueFactory(Uri uri);
        IQueue GetQueue(string uri);
        IQueue CreateQueue(string uri);
        IQueue CreateQueue(Uri uri);
	    void CreatePhysicalQueues(IServiceBusConfiguration serviceBusConfiguration, QueueCreationType queueCreationType);
        IEnumerable<IQueueFactory> GetQueueFactories();
        void RegisterQueueFactory(IQueueFactory queueFactory);
        bool ContainsQueueFactory(string scheme);
    }
}