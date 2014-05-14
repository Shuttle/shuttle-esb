using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.RabbitMQ
{
	internal class Channel : IDisposable
	{
		private readonly string _queue;
		private Subscription _subscription;
		private readonly int _millisecondsTimeout;

		public Channel(IModel model, RabbitMQUriParser parser, IRabbitMQConfiguration configuration)
		{
			Guard.AgainstNull(model, "model");
			Guard.AgainstNull(parser, "parser");
			Guard.AgainstNull(configuration, "configuration");

			Model = model;

			_queue = parser.Queue;

			_millisecondsTimeout = parser.Local
						   ? configuration.LocalQueueTimeoutMilliseconds
						   : configuration.RemoteQueueTimeoutMilliseconds;
		}

		public IModel Model { get; private set; }

		public BasicDeliverEventArgs Next()
		{
			BasicDeliverEventArgs basicDeliverEventArgs;

			var next = GetSubscription().Next(_millisecondsTimeout, out basicDeliverEventArgs);

			if (next && basicDeliverEventArgs == null)
			{
				throw new ConnectionException(string.Format(RabbitMQResources.SubscriptionNextConnectionException, _subscription.QueueName));
			}

			return (next)
					   ? basicDeliverEventArgs
					   : null;
		}

		private Subscription GetSubscription()
		{
			return _subscription ?? (_subscription = new Subscription(Model, _queue, false));
		}

		public void Acknowledge(BasicDeliverEventArgs basicDeliverEventArgs)
		{
			GetSubscription().Ack(basicDeliverEventArgs);
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