using System;
using System.IO;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class ResolvedQueue : IQueue
	{
		private readonly IQueue _queue;

		public ResolvedQueue(IQueue queue, Uri uri)
		{
			Guard.AgainstNull(queue,"queue");
			Guard.AgainstNull(uri, "uri");

			_queue = queue;
			Uri = uri;
		}

		public Uri Uri { get; private set; }

		public bool IsEmpty()
		{
			return _queue.IsEmpty();
		}

		public void Enqueue(Guid messageId, Stream stream)
		{
			_queue.Enqueue(messageId, stream);
		}

		public ReceivedMessage GetMessage()
		{
			return _queue.GetMessage();
		}

		public void Acknowledge(object acknowledgementToken)
		{
			_queue.Acknowledge(acknowledgementToken);
		}

		public void Release(object acknowledgementToken)
		{
			_queue.Release(acknowledgementToken);
		}
	}
}