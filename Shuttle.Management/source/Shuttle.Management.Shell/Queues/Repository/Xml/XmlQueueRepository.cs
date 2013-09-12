using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
    public class XmlQueueRepository : IQueueRepository
    {
    	private readonly IObjectSerializer serializer;
    	private readonly string queueRepositoryPath;
    	private readonly XmlQueueCollection queues;

    	public XmlQueueRepository()
    	{
    		queueRepositoryPath = ConfigurationItem<string>.ReadSetting("XmlQueueRepositoryPath").GetValue();

			serializer = new XmlObjectSerializer();
			
			if (!File.Exists(queueRepositoryPath))
			{
				queues = new XmlQueueCollection();

				File.WriteAllText(queueRepositoryPath, serializer.Serialize(queues));

				return;
			}

			queues = serializer.Deserialize<XmlQueueCollection>(File.ReadAllText(queueRepositoryPath));
    	}

    	public IEnumerable<Queue> All()
    	{
    		return new ReadOnlyCollection<Queue>(queues);
    	}

        public void Save(Queue queue)
        {
			Guard.AgainstNull(queue, "queue");

            if (Contains(queue.Uri))
            {
            	return;
            }

			queues.Add(queue);

			PersistStore();
        }

    	private void PersistStore()
    	{
			queues.Sort();

			File.WriteAllText(queueRepositoryPath, serializer.Serialize(queues));
		}

    	public void Remove(string uri)
        {
    		var queue = Get(uri);

			if (queue == null)
			{
				return;
			}

    		queues.Remove(queue);

    		PersistStore();
        }

    	public bool Contains(string uri)
    	{
    		return Get(uri) != null;
    	}

    	public Queue Get(string uri)
    	{
    		return queues.Find(queue=> queue.Uri.Equals(uri, StringComparison.InvariantCultureIgnoreCase));
    	}
    }
}