using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class MemoryQueueFactory : IQueueFactory
    {
        public string Scheme
        {
            get
            {
                return MemoryQueue.SCHEME;
            }
        }

        public IQueue Create(Uri uri)
        {
            Guard.AgainstNull(uri, "uri");

            return new MemoryQueue(uri);
        }

        public bool CanCreate(Uri uri)
        {
            Guard.AgainstNull(uri, "uri");

            return Scheme.Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}