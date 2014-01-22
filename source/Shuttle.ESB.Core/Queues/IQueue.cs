using System;
using System.IO;

namespace Shuttle.ESB.Core
{
    public interface IQueue
    {
        Uri Uri { get; }

		bool IsEmpty();

	    void Enqueue(Guid messageId, Stream stream);
        Stream Dequeue();
        Stream Dequeue(Guid messageId);
    	bool Remove(Guid messageId);
    }
}