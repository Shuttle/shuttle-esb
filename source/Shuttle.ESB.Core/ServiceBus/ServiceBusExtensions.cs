using System;
using System.Collections.Generic;
using System.Linq;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public static class ServiceBusExtensions
	{
		public static void AttemptInitialization(this object o, IServiceBus bus)
		{
			if (o == null || bus == null)
			{
				return;
			}

			var required = o as IRequireInitialization;

			if (required == null)
			{
				return;
			}

			required.Initialize(bus);
		}


	//	public static TransportMessage Send(this IServiceBus bus, object message)
	//	{
	//		return Send(bus, DateTime.MinValue, message, null);
	//	}

	//	public static TransportMessage Send(this IServiceBus bus, object message, IQueue queue)
	//	{
	//		return Send(bus, DateTime.MinValue, message, queue);
	//	}

	//	public static TransportMessage Send(this IServiceBus bus, DateTime at, object message, IQueue queue)
	//	{
	//		Guard.AgainstNull(message, "message");

	//		var messagePipeline = bus.Configuration.PipelineFactory.GetPipeline<TransportMessagePipeline>(bus);

	//		if (_log.IsTraceEnabled)
	//		{
	//			_log.Trace(string.Format(ESBResources.TraceSend, message.GetType().FullName,
	//									 queue == null ? "[null]" : queue.Uri.ToString()));
	//		}

	//		try
	//		{
	//			messagePipeline.Execute(new TransportMessageConfigurator(message)
	//				{
	//					IgnoreTillDate = at,
	//					Queue = queue
	//				});

	//			return messagePipeline.State.Get<TransportMessage>(StateKeys.TransportMessage);
	//		}
	//		finally
	//		{
	//			bus.Configuration.PipelineFactory.ReleasePipeline(messagePipeline);
	//		}
	//	}

	//	public static IEnumerable<string> Publish(this IServiceBus bus, object message)
	//	{
	//		Guard.AgainstNull(message, "message");

	//		if (bus.Configuration.HasSubscriptionManager)
	//		{
	//			var subscribers = bus.Configuration.SubscriptionManager.GetSubscribedUris(message).ToList();

	//			if (subscribers.Count > 0)
	//			{
	//				var result = new List<string>();

	//				foreach (var subscriber in subscribers)
	//				{
	//					Send(bus, message, bus.Configuration.QueueManager.GetQueue(subscriber));

	//					result.Add(subscriber);
	//				}

	//				return result;
	//			}

	//			_log.Warning(string.Format(ESBResources.WarningPublishWithoutSubscribers, message.GetType().FullName));
	//		}
	//		else
	//		{
	//			throw new InvalidOperationException(string.Format(ESBResources.PublishWithoutSubscriptionManagerException,
	//															  message.GetType().FullName));
	//		}

	//		return EmptyPublishFlyweight;
	//	}
	}
}