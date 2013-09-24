using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class MemoryQueue : IQueue, ICount, ICreate, IPurge
	{
		internal const string SCHEME = "memory";

		[ThreadStatic]
		private static KeyValuePair<Guid, Stream> underlyingMessageData;

		private static readonly object padlock = new object();
		public static IQueue Null = new NullMemoryQueue();
		private static Dictionary<string, Dictionary<Guid, Stream>> queues = new Dictionary<string, Dictionary<Guid, Stream>>();

		public MemoryQueue(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			if (!uri.Scheme.Equals(SCHEME, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new InvalidSchemeException(SCHEME, uri.ToString());
			}

			var builder = new UriBuilder(uri);

			if (uri.Host.Equals("."))
			{
				builder.Host = Environment.MachineName.ToLower();
			}

			if (uri.LocalPath == "/")
			{
				builder.Path = "/default";
			}

			Uri = builder.Uri;

			if (Uri.Host != Environment.MachineName.ToLower())
			{
				throw new UriFormatException(string.Format(ESBResources.UriFormatException,
														   string.Format("memory://{{.|{0}}}/{{name}}",
																		 Environment.MachineName.ToLower()), uri));
			}

            Create();
		}

		public int Count
		{
			get
			{
				lock (padlock)
				{
					return queues[Uri.ToString()].Count;
				}
			}
		}

		public Uri Uri { get; private set; }

		public QueueAvailability Exists()
		{
			return QueueAvailability.Exists;
		}

		public bool IsEmpty()
		{
			return Count == 0;
		}

		public object UnderlyingMessageData
		{
			get { return underlyingMessageData; }
		}

		public void Enqueue(object data)
		{
			lock (padlock)
			{
				var kvp = (KeyValuePair<Guid, Stream>)data;

				queues[Uri.ToString()].Add(kvp.Key, kvp.Value);
			}
		}

		public void Enqueue(Guid messageId, Stream stream)
		{
			lock (padlock)
			{
				queues[Uri.ToString()].Add(messageId, stream.Copy());
			}
		}

		public Stream Dequeue()
		{
			lock (padlock)
			{
				var queue = queues[Uri.ToString()];

				underlyingMessageData = queue.Count != 0
											? queue.ElementAt(0)
											: new KeyValuePair<Guid, Stream>(Guid.Empty, null);

				queue.Remove(underlyingMessageData.Key);

				return underlyingMessageData.Value;
			}
		}

		public Stream Dequeue(Guid messageId)
		{
			lock (padlock)
			{
				var queue = queues[Uri.ToString()];

				if (queue.ContainsKey(messageId))
				{
					underlyingMessageData = new KeyValuePair<Guid, Stream>(messageId, queue[messageId]);

					queue.Remove(messageId);

					return underlyingMessageData.Value;
				}

				return null;
			}
		}

		public bool Remove(Guid messageId)
		{
			lock (padlock)
			{
				var queue = queues[Uri.ToString()];

				return queue.ContainsKey(messageId) && queue.Remove(messageId);
			}
		}

		public bool IsLocal
		{
			get { return true; }
		}

		public bool IsTransactional
		{
			get { return false; }
		}

		public static IQueue From(string uri)
		{
			return new MemoryQueue(new Uri(uri));
		}

		public static void Clear()
		{
			queues = new Dictionary<string, Dictionary<Guid, Stream>>();
		}

	    public void Create()
	    {
            if (!queues.ContainsKey(Uri.ToString()))
            {
                queues.Add(Uri.ToString(), new Dictionary<Guid, Stream>());
            }
        }

	    public void Purge()
	    {
            lock (padlock)
            {
                queues[Uri.ToString()].Clear();
            }
        }
	}

	public class NullMemoryQueue : IQueue
	{
		internal NullMemoryQueue()
		{
			Uri = new Uri("memory://./null");
		}

		public QueueAvailability Exists()
		{
			return QueueAvailability.Exists;
		}

		public bool IsEmpty()
		{
			return true;
		}

		public object UnderlyingMessageData
		{
			get { return null; }
		}

		public void Enqueue(object data)
		{
		}

		public void Enqueue(Guid messageId, Stream stream)
		{
		}

		public Stream Dequeue()
		{
			return null;
		}

		public Stream Dequeue(Guid messageId)
		{
			return null;
		}

		public bool Remove(Guid messageId)
		{
			return false;
		}

		public bool IsLocal
		{
			get { return true; }
		}

		public bool IsTransactional
		{
			get { return false; }
		}

		public Uri Uri { get; private set; }
	}
}