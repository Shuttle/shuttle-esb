using System;

namespace Shuttle.ESB.Core
{
    public class QueueFactoryNotFoundException : Exception
    {
        public QueueFactoryNotFoundException(string scheme)
            : base(string.Format(ESBResources.QueueFactoryNotFoundException, scheme))
        {
        }
    }
}