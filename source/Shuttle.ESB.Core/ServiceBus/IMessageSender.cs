using System;
using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
	public interface IMessageSender
	{
		TransportMessage CreateTransportMessage(object message, Action<TransportMessageConfigurator> configure);

		void Dispatch(TransportMessage transportMessage);
		TransportMessage Send(object message);
		TransportMessage Send(object message, Action<TransportMessageConfigurator> configure);
		IEnumerable<TransportMessage> Publish(object message);
		IEnumerable<TransportMessage> Publish(object message, Action<TransportMessageConfigurator> configure);
	}
}