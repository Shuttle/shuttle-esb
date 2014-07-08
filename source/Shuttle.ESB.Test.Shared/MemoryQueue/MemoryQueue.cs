using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class MemoryQueue : IQueue, ICreateQueue, IPurgeQueue
	{
		internal const string SCHEME = "memory";

		private static readonly object _padlock = new object();
		private static Dictionary<string, Dictionary<int, MemoryQueueItem>> _queues = new Dictionary<string, Dictionary<int, MemoryQueueItem>>();
		private readonly List<int> _unacknowledgedMessageIds = new List<int>();
		private int _itemId = 0;

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

		public Uri Uri { get; private set; }

		public bool IsEmpty()
		{
			lock (_padlock)
			{
				return _queues[Uri.ToString()].Count == 0;
			}
		}

		public void Enqueue(Guid messageId, Stream stream)
		{
			lock (_padlock)
			{
				_itemId++;

				_queues[Uri.ToString()].Add(_itemId, new MemoryQueueItem(_itemId, messageId, stream.Copy()));
			}
		}

		public ReceivedMessage GetMessage()
		{
			lock (_padlock)
			{
				var queue = _queues[Uri.ToString()];

				var index = 0;

				while (index < queue.Count)
				{
					var pair = queue.ElementAt(index);

					if (!_unacknowledgedMessageIds.Contains(pair.Value.ItemId))
					{
						_unacknowledgedMessageIds.Add(pair.Value.ItemId);

						return new ReceivedMessage(pair.Value.Stream, pair.Value.ItemId);
					}
					
					index++;
				}

				return null;
			}
		}

		public void Acknowledge(object acknowledgementToken)
		{
			var itemId = (int)acknowledgementToken;

			lock (_padlock)
			{
				var queue = _queues[Uri.ToString()];

				if (!queue.ContainsKey(itemId) || !_unacknowledgedMessageIds.Contains(itemId))
				{
					return;
				}

				if (queue.ContainsKey(itemId))
				{
					queue.Remove(itemId);
				}

				_unacknowledgedMessageIds.Remove(itemId);
			}
		}

		public void Release(object acknowledgementToken)
		{
			var itemId = (int) acknowledgementToken;

			lock (_padlock)
			{
				var queue = _queues[Uri.ToString()];

				if (!queue.ContainsKey(itemId) || !_unacknowledgedMessageIds.Contains(itemId))
				{
					return;
				}

				if (queue.ContainsKey(itemId))
				{
					var message = queue[itemId];

					queue.Remove(itemId);

					queue.Add(itemId, message);
				}

				_unacknowledgedMessageIds.Remove(itemId);
			}
		}

		public bool IsLocal
		{
			get { return true; }
		}

		public static IQueue From(string uri)
		{
			return new MemoryQueue(new Uri(uri));
		}

		public static void Clear()
		{
			_queues = new Dictionary<string, Dictionary<int, MemoryQueueItem>>();
		}

	    public void Create()
	    {
            if (!_queues.ContainsKey(Uri.ToString()))
            {
                _queues.Add(Uri.ToString(), new Dictionary<int, MemoryQueueItem>());
            }
        }

	    public void Purge()
	    {
            lock (_padlock)
            {
                _queues[Uri.ToString()].Clear();
            }
        }
	}

	internal class MemoryQueueItem
	{
		public int ItemId { get; private set; }
		public Guid MessageId { get; private set; }
		public Stream Stream { get; private set; }

		public MemoryQueueItem(int index, Guid messageId, Stream stream)
		{
			ItemId = index;
			MessageId = messageId;
			Stream = stream;
		}
	}
}