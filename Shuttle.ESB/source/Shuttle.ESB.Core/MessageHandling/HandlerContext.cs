using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class HandlerContext<T> where T : class
	{
		public HandlerContext(IServiceBus bus, TransportMessage transportMessage, T message, IActiveState activeState)
		{
			Guard.AgainstNull(bus, "bus");
			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNull(message, "message");
			Guard.AgainstNull(activeState, "activeState");

			Bus = bus;
			TransportMessage = transportMessage;
			Message = message;
			ActiveState = activeState;
		}

		public IServiceBus Bus { get; private set; }
		public TransportMessage TransportMessage { get; private set; }
		public T Message { get; private set; }
		public IActiveState ActiveState { get; private set; }
	}
}