using System;
using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
	public interface IServiceBus : IDisposable
	{
		void HandlingTransportMessage(TransportMessage transportMessage);
		void TransportMessageHandled();

		string OutgoingCorrelationId { get; set; }
		List<TransportHeader> OutgoingHeaders { get; }
		void ResetOutgoingHeaders();

		TransportMessage CreateTransportMessage(object message);

		void Send(TransportMessage transportMessage);

		TransportMessage Send(object message);
		TransportMessage Send(object message, string uri);
		TransportMessage Send(object message, IQueue queue);

		TransportMessage SendLocal(object message);
		TransportMessage SendReply(object message);

		TransportMessage SendDeferred(DateTime at, object message);
		TransportMessage SendDeferred(DateTime at, object message, string uri);
		TransportMessage SendDeferred(DateTime at, object message, IQueue queue);

		TransportMessage SendDeferredLocal(DateTime at, object message);
		TransportMessage SendDeferredReply(DateTime at, object message);

		IEnumerable<string> Publish(object message);

		IServiceBus Start();
		void Stop();
		bool Started { get; }

		IServiceBusConfiguration Configuration { get; }
		IServiceBusEvents Events { get; }
		TransportMessage TransportMessageBeingHandled { get; }
		bool IsHandlingTransportMessage { get; }
	}
}