using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class MessageRouteCollection : IMessageRouteCollection
    {
        private readonly List<IMessageRoute> maps = new List<IMessageRoute>();

        public IMessageRouteCollection Add(IMessageRoute map)
        {
            Guard.AgainstNull(map, "map");

            var existing = Find(map.Queue);

            if (existing == null)
            {
                maps.Add(map);
            }
            else
            {
                foreach (var specification in map.Specifications)
                {
                    existing.AddSpecification(specification);
                }
            }

            return this;
        }

        public List<IMessageRoute> FindAll(string messageType)
        {
            Guard.AgainstNull(messageType, "message");

        	return maps.Where(map => map.IsSatisfiedBy(messageType)).ToList();
        }

    	public IMessageRoute Find(Uri uri)
        {
            Guard.AgainstNull(uri, "uri");

            return Find(uri.ToString());
        }

        public IMessageRoute Find(string uri)
        {
            return maps.Find(map => map.Queue.Uri.ToString().Equals(uri, StringComparison.InvariantCultureIgnoreCase));
        }

        public IMessageRoute Find(IQueue queue)
        {
            Guard.AgainstNull(queue, "queue");

            return Find(queue.Uri.ToString());
        }

        public IEnumerator<IMessageRoute> GetEnumerator()
        {
            return maps.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}