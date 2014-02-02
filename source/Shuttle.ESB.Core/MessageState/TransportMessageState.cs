using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
	public class TransportMessageState
	{
		public TransportMessageState()
		{
			MessagesToSend = new List<TransportMessage>();
		}

		public bool HasMessageBeenHandled { get; set; }

		public List<TransportMessage> MessagesToSend { get; set; }
	}
}