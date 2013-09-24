using System;
using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
    public interface IMessageHandlerFactory : IRequireInitialization
    {
    	IMessageHandler GetHandler(object message);
    	void ReleaseHandler(IMessageHandler handler);

        IEnumerable<Type> MessageTypesHandled { get; }
    }
}