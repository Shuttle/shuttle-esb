using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
	public interface IReceiveMessageStateService
    {
        bool HasMessageBeenHandled(TransportMessage transportMessage);
		void HandleMessage(TransportMessage transportMessage);
		void AcknowledgeMessage(TransportMessage transportMessage);
		IEnumerable<TransportMessage> GetMessagesToSend(TransportMessage transportMessage);
		void RegisterMessageToSend(TransportMessage transportMessage, TransportMessage transportMessageToSend);
		void RegisterSent(TransportMessage transportMessage, TransportMessage transportMessageSent);
    }
}