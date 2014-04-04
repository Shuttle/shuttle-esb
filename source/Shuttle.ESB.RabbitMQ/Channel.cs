using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMQ
{
	internal class Channel : IDisposable
	{
		private readonly Subscription _subscription;
		private readonly int _millisecondsTimeout;
		private readonly bool _consume;
		private readonly Uri _uri;

		public Channel(IModel model, RabbitMQUriParser parser, IRabbitMQConfiguration configuration)
		{
			Guard.AgainstNull(model, "model");
			Guard.AgainstNull(parser, "parser");
			Guard.AgainstNull(configuration, "configuration");

			Model = model;

			_consume = parser.Consume;
			_uri = parser.Uri.Secured();

			_millisecondsTimeout = parser.Local
						   ? configuration.LocalQueueTimeoutMilliseconds
						   : configuration.RemoteQueueTimeoutMilliseconds;

			if (_consume)
			{
				_subscription = new Subscription(model, parser.Queue, false);
			}
		}

		public IModel Model { get; private set; }

		public BasicDeliverEventArgs Next()
		{
			if (!_consume)
			{
				throw new RabbitMQQueueException(string.Format(RabbitMQResources.ConsumeException, _uri));
			}

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
			if (!_consume)
			{
				throw new RabbitMQQueueException(string.Format(RabbitMQResources.ConsumeException, _uri));
			}

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