using System;

namespace Shuttle.ESB.Core
{
    public class DuplicateQueueFactoryException : Exception
    {
        public DuplicateQueueFactoryException(IQueueFactory factory)
            : base(string.Format(ESBResources.DuplicateQueueFactoryException, factory.Scheme, factory.GetType().FullName))
        {
        }
    }
}