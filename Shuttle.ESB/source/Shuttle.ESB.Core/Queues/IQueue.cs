using System;
using System.IO;

namespace Shuttle.ESB.Core
{
	public enum QueueAvailability
	{
		Missing = 0,
		Exists = 1,
		Unknown =2
	}

    public interface IQueue
    {
        bool IsLocal { get; }
        bool IsTransactional { get; }
        Uri Uri { get; }
        QueueAvailability Exists();
    	bool IsEmpty();

        object UnderlyingMessageData { get; }

        void Enqueue(object data);
        void Enqueue(Guid messageId, Stream stream);
        Stream Dequeue();
        Stream Dequeue(Guid messageId);
    	bool Remove(Guid messageId);
    }
}