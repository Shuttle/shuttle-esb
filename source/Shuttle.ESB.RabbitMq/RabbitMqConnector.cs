using System.Collections.Generic;
using System.Threading;
using RabbitMQ.Client;

namespace Shuttle.ESB.RabbitMq
{
	public class RabbitMqConnector
	{
		private readonly Dictionary<int, IModel> _channels = new Dictionary<int, IModel>();
		private readonly object _padLock = new object();
		private readonly IConnection _connection;

		public RabbitMqConnector(RabbitMqQueuePath queuePath)
		{
			QueuePath = queuePath;
			var factory = new ConnectionFactory {uri = QueuePath.ConnnectUri, Port = AmqpTcpEndpoint.UseDefaultPort};
			_connection = factory.CreateConnection();
		}

		public IModel RequestChannel()
		{
			lock (_padLock)
			{
				IModel channel;

				if (!_channels.TryGetValue(Thread.CurrentThread.ManagedThreadId, out channel))
				{
					channel = _connection.CreateModel();

					if (!string.IsNullOrEmpty(QueuePath.Exchange))
					{
						channel.ExchangeDeclare(QueuePath.Exchange, ExchangeType.Direct, true);
					}

					_channels.Add(Thread.CurrentThread.ManagedThreadId, channel);
				}
				return channel;
			}
		}

		public void Close()
		{
			foreach (var channel in _channels)
			{
				channel.Value.Close();
			}

			_connection.Close();
		}

		public RabbitMqQueuePath QueuePath { get; private set; }
	}
}