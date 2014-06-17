using System;
using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
    public interface IMessageRouteCollection : IEnumerable<IMessageRoute>
    {
        IMessageRouteCollection Add(IMessageRoute map);

        List<IMessageRoute> FindAll(string messageType);
    	IMessageRoute Find(Uri uri);
        IMessageRoute Find(string uri);
        IMessageRoute Find(IQueue queue);
    }
}