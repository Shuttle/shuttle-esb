using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class MessageSender : IMessageSender
	{
		private static readonly IEnumerable<TransportMessage> EmptyPublishFlyweight = new ReadOnlyCollection<TransportMessage>(new List<TransportMessage>());

		private readonly IServiceBus _bus;
		private readonly TransportMessage _transportMessageReceived;

		private readonly ILog _log;

		public MessageSender(IServiceBus bus)
			: this(bus, null)
		{
		}

		public MessageSender(IServiceBus bus, TransportMessage transportMessageReceived)
		{
			Guard.AgainstNull(bus, "bus");

			_bus = bus;
			_transportMessageReceived = transportMessageReceived;

			_log = Log.For(this);
		}

		public TransportMessage CreateTransportMessage(object message, Action<TransportMessageConfigurator> configure)
		{
			Guard.AgainstNull(message, "message");

			var transportMessagePipeline = _bus.Configuration.PipelineFactory.GetPipeline<TransportMessagePipeline>(_bus);

			var transportMessageConfigurator = new TransportMessageConfigurator(message);

			if (_transportMessageReceived != null)
			{
				transportMessageConfigurator.TransportMessageReceived(_transportMessageReceived);
			}

			if (configure != null)
			{
				configure(transportMessageConfigurator);
			}

			if (!transportMessagePipeline.Execute(transportMessageConfigurator))
			{
				throw new PipelineException(string.Format(ESBResources.PipelineExecutionException, "TransportMessagePipeline",
														  transportMessagePipeline.Exception.AllMessages()));
			}

			return transportMessagePipeline.State.GetTransportMessage();
		}

		public void Dispatch(TransportMessage transportMessage)
		{
			Guard.AgainstNull(transportMessage, "transportMessage");

			var messagePipeline = _bus.Configuration.PipelineFactory.GetPipeline<DispatchTransportMessagePipeline>(_bus);

			try
			{
				messagePipeline.Execute(new MessageSenderContext(transportMessage, _transportMessageReceived));
			}
			finally
			{
				_bus.Configuration.PipelineFactory.ReleasePipeline(messagePipeline);
			}
		}

		public TransportMessage Send(object message)
		{
			return Send(message, null);
		}

		public TransportMessage Send(object message, Action<TransportMessageConfigurator> configure)
		{
			Guard.AgainstNull(message, "message");

			var result = CreateTransportMessage(message, configure);

			Dispatch(result);

			return result;
		}

		public IEnumerable<TransportMessage> Publish(object message)
		{
			return Publish(message, null);
		}

		public IEnumerable<TransportMessage> Publish(object message, Action<TransportMessageConfigurator> configure)
		{
			Guard.AgainstNull(message, "message");

			if (_bus.Configuration.HasSubscriptionManager)
			{
				var subscribers = _bus.Configuration.SubscriptionManager.GetSubscribedUris(message).ToList();

				if (subscribers.Count > 0)
				{
					var result = new List<TransportMessage>();

					foreach (var subscriber in subscribers)
					{
						var transportMessage = CreateTransportMessage(message, configure);

						transportMessage.RecipientInboxWorkQueueUri = subscriber;

						Dispatch(transportMessage);

						result.Add(transportMessage);
					}

					return result;
				}

				_log.Warning(string.Format(ESBResources.WarningPublishWithoutSubscribers, message.GetType().FullName));
			}
			else
			{
				throw new InvalidOperationException(string.Format(ESBResources.PublishWithoutSubscriptionManagerException,
																  message.GetType().FullName));
			}

			return EmptyPublishFlyweight;
		}
	}
}