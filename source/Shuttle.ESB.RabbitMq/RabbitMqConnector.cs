using System;
using RabbitMQ.Client;

namespace Shuttle.ESB.RabbitMq
{
	internal class RabbitMqConnector
	{
		public RabbitMqConnector(Uri uri)
		{
			Factory = new ConnectionFactory {uri = uri, Port = AmqpTcpEndpoint.UseDefaultPort};
			Connection = Factory.CreateConnection();
		}

		public ConnectionFactory Factory { get; private set; }
		public IConnection Connection { get; private set; }
	}
}