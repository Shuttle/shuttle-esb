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
		private readonly RabbitMqExchangeElement _exchangeConfiguration;

		public RabbitMqConnector(RabbitMqExchangeElement exchangeConfiguration, RabbitMqQueueElement queueConfiguration, RabbitMqQueuePath queuePath)
		{
			_exchangeConfiguration = exchangeConfiguration;
			QueueConfiguration = queueConfiguration;
			QueuePath = queuePath;

			var port = QueuePath.ConnnectUri.Port < 1
				? AmqpTcpEndpoint.UseDefaultPort
				: QueuePath.ConnnectUri.Port;

			var factory = new ConnectionFactory() {uri = QueuePath.ConnnectUri, Port = port};

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

					if (_exchangeConfiguration != null)
					{
						channel.ExchangeDeclare(_exchangeConfiguration.Name, _exchangeConfiguration.Type, _exchangeConfiguration.IsDurable, _exchangeConfiguration.AutoDelete, null);
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

		public RabbitMqQueueElement QueueConfiguration { get; private set; }
	}
}