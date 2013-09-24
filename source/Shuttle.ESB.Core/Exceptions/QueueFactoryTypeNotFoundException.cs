using System;

namespace Shuttle.ESB.Core
{
    public class QueueFactoryTypeNotFoundException : Exception
    {
        public QueueFactoryTypeNotFoundException(string type, string scheme)
            : base(string.Format(ESBResources.QueueFactoryTypeNotFoundException, type, scheme))
        {
        }
    }
}