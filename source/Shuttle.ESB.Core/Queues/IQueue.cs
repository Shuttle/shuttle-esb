using System;
using System.IO;

namespace Shuttle.ESB.Core
{
    public interface IQueue
    {
        Uri Uri { get; }

		bool IsEmpty();

	    void Enqueue(Guid messageId, Stream stream);
        Stream GetMessage();
        Stream GetMessage(Guid messageId);
	    void Acknowledge(Guid messageId);
	    void Release(Guid messageId);
    }
}