using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.RabbitMQ
{
	internal class Channel : IDisposable
	{
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
			BasicDeliverEventArgs basicDeliverEventArgs;

			var next = _subscription.Next(_millisecondsTimeout, out basicDeliverEventArgs);

			if (next && basicDeliverEventArgs == null)
			{
				throw new ConnectionException(string.Format(RabbitMQResources.SubscriptionNextConnectionException, _subscription.QueueName));
			}

			return (next)
				       ? basicDeliverEventArgs
				       : null;
		}

		public void Acknowledge(BasicDeliverEventArgs basicDeliverEventArgs)
		{
			_subscription.Ack(basicDeliverEventArgs);
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