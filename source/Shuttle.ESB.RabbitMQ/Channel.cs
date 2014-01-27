using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMQ
{
	internal class Channel : IAcknowledge, IDisposable
	{
		private BasicDeliverEventArgs _basicDeliverEventArgs;
		private readonly Subscription _subscription;
		private readonly int _millisecondsTimeout;

		public Channel(IModel model, Subscription subscription, int millisecondsTimeout)
		{
			Guard.AgainstNull(model, "model");
			Guard.AgainstNull(subscription, "subscription");

			Model = model;
			_subscription = subscription;
			_millisecondsTimeout = millisecondsTimeout;
		}

		public IModel Model { get; private set; }

		public BasicDeliverEventArgs Next()
		{
			_basicDeliverEventArgs = null;

			var next = _subscription.Next(_millisecondsTimeout, out _basicDeliverEventArgs);

			if (next && _basicDeliverEventArgs == null)
			{
				throw new ConnectionException(string.Format(RabbitMQResources.SubscriptionNextConnectionException, _subscription.QueueName));
			}

			return (next)
				       ? _basicDeliverEventArgs
				       : null;
		}

		public void Acknowledge()
		{
			_subscription.Ack(_basicDeliverEventArgs);
		}

		public void Dispose()
		{
			if (Model.IsOpen)
			{
				Model.Close();
			}

			Model.Dispose();
		}
	}
}