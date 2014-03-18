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

		private static readonly object _padlock = new object();
		private static Dictionary<string, Dictionary<Guid, Stream>> _queues = new Dictionary<string, Dictionary<Guid, Stream>>();
		private readonly List<Guid> _unacknowledgedMessageIds = new List<Guid>();

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
				lock (_padlock)
				{
					return _queues[Uri.ToString()].Count;
				}
			}
		}

		public Uri Uri { get; private set; }

		public bool IsEmpty()
		{
			return Count == 0;
		}

		public void Enqueue(Guid messageId, Stream stream)
		{
			lock (_padlock)
			{
				_queues[Uri.ToString()].Add(messageId, stream.Copy());
			}
		}

		public Stream Dequeue()
		{
			lock (_padlock)
			{
				var queue = _queues[Uri.ToString()];

				var index = 0;

				while (index < queue.Count)
				{
					var pair = queue.ElementAt(index);

					if (!_unacknowledgedMessageIds.Contains(pair.Key))
					{
						_unacknowledgedMessageIds.Add(pair.Key);

						return pair.Value;
					}
				}

				return null;
			}
		}

		public Stream Dequeue(Guid messageId)
		{
			lock (_padlock)
			{
				var queue = _queues[Uri.ToString()];

				if (queue.ContainsKey(messageId))
				{
					if (!_unacknowledgedMessageIds.Contains(messageId))
					{
						_unacknowledgedMessageIds.Add(messageId);
					}

					return queue[messageId];
				}

				return null;
			}
		}

		public void Acknowledge(Guid messageId)
		{
			lock (_padlock)
			{
				var queue = _queues[Uri.ToString()];

				if (!queue.ContainsKey(messageId) || !_unacknowledgedMessageIds.Contains(messageId))
				{
					return;
				}

				if (queue.ContainsKey(messageId))
				{
					queue.Remove(messageId);
				}

				_unacknowledgedMessageIds.Remove(messageId);
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
			_queues = new Dictionary<string, Dictionary<Guid, Stream>>();
		}

	    public void Create()
	    {
            if (!_queues.ContainsKey(Uri.ToString()))
            {
                _queues.Add(Uri.ToString(), new Dictionary<Guid, Stream>());
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
}