using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class HandlerContext<T> : IMessageSender
		where T : class
	{
		private readonly IServiceBus _bus;
		private readonly IMessageSender _messageSender;

		public HandlerContext(IServiceBus bus, TransportMessage transportMessage, T message, IThreadState activeState)
		{
			Guard.AgainstNull(bus, "bus");
			Guard.AgainstNull(transportMessage, "transportMessage");
			Guard.AgainstNull(message, "message");
			Guard.AgainstNull(activeState, "activeState");

			_bus = bus;
			_messageSender = new MessageSender(bus, transportMessage);

			TransportMessage = transportMessage;
			Message = message;
			ActiveState = activeState;
			Configuration = _bus.Configuration;
		}

		public TransportMessage TransportMessage { get; private set; }
		public T Message { get; private set; }
		public IThreadState ActiveState { get; private set; }
		public IServiceBusConfiguration Configuration { get; private set; }

		public TransportMessage CreateTransportMessage(object message, Action<TransportMessageConfigurator> configurator)
		{
			return _messageSender.CreateTransportMessage(message, configurator);
		}

		public void Dispatch(TransportMessage transportMessage)
		{
			_messageSender.Dispatch(transportMessage);
		}

		public TransportMessage Send(object message)
		{
			return _messageSender.Send(message);
		}

		public TransportMessage Send(object message, Action<TransportMessageConfigurator> configurator)
		{
			return _messageSender.Send(message, configurator);
		}

		public IEnumerable<TransportMessage> Publish(object message)
		{
			return _messageSender.Publish(message);
		}

		public IEnumerable<TransportMessage> Publish(object message, Action<TransportMessageConfigurator> configurator)
		{
			return _messageSender.Publish(message, configurator);
		}

		public TransportMessage SendReply(object message)
		{
			if (string.IsNullOrEmpty(TransportMessage.SenderInboxWorkQueueUri))
			{
				throw new InvalidOperationException(ESBResources.SendReplyException);
			}

			return _messageSender.Send(message, c => c.SendToRecipient(TransportMessage.SenderInboxWorkQueueUri));
		}

		public TransportMessage SendReply(object message, Action<TransportMessageConfigurator> configurator)
		{
			if (string.IsNullOrEmpty(TransportMessage.SenderInboxWorkQueueUri))
			{
				throw new InvalidOperationException(ESBResources.SendReplyException);
			}

			Action<TransportMessageConfigurator> reply = c=> c.SendToRecipient(TransportMessage.SenderInboxWorkQueueUri);

			if (configurator != null)
			{
				reply += configurator;
			}

			return _messageSender.Send(message, reply);
		}
	}
}
