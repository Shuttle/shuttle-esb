using System;
using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
    public interface IQueues
    {
        IQueueFactory GetQueueFactory(string uri);
        IQueueFactory GetQueueFactory(Uri uri);
        IQueue GetQueue(string uri);
        IQueue CreateQueue(string uri);
        IQueue CreateQueue(Uri uri);
        void CreatePhysicalQueue(IQueue queue);
        void CreatePhysicalQueues(QueueCreationType queueCreationType);
        void Purge(IQueue queue);
        IEnumerable<IQueueFactory> GetQueueFactories();
        void RegisterQueueFactory(IQueueFactory queueFactory);
        bool ContainsQueueFactory(string scheme);
    }
}