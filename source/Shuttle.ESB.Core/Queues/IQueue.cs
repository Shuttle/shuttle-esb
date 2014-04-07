using System;
using System.IO;

namespace Shuttle.ESB.Core
{
    public interface IQueue
    {
        Uri Uri { get; }

		bool IsEmpty();

	    void Enqueue(Guid messageId, Stream stream);
        ReceivedMessage GetMessage();
	    void Acknowledge(object acknowledgementToken);
	    void Release(object acknowledgementToken);
    }
}