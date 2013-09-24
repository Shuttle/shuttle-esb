using System;

namespace Shuttle.ESB.Core
{
    public class QueueDoesNotExistException : Exception
    {
        public QueueDoesNotExistException(string uri)
            : base(string.Format(ESBResources.QueueDoesNotExistException, uri))
        {
        }
    }
}