using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
    public interface ISubscriptionManager
    {
        void Subscribe(IEnumerable<string> messageTypes);

        IEnumerable<string> GetSubscribedUris(object message);
    }
}