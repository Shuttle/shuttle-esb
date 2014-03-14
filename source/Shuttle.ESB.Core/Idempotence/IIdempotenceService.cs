using System.Collections.Generic;
using System.IO;

namespace Shuttle.ESB.Core
{
	public interface IIdempotenceService
	{
		bool ShouldProcess(TransportMessage transportMessage);
		void ProcessingCompleted(TransportMessage transportMessage);
		void AddDeferredMessage(TransportMessage processingTransportMessage, Stream deferredTransportMessageStream);
		IEnumerable<Stream> GetDeferredMessages(TransportMessage transportMessage);
		void DeferredMessageSent(TransportMessage processingTransportMessage, TransportMessage deferredTransportMessage);
	}
}