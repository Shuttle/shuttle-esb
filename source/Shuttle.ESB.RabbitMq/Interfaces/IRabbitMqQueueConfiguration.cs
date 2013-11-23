using System;

namespace Shuttle.ESB.RabbitMq.Interfaces
{
	public interface IRabbitMqQueueConfiguration
	{
		IRabbitMqQueueConfiguration IsDurable(bool value);
		IRabbitMqQueueConfiguration IsTransactional(bool value);
		IRabbitMqQueueConfiguration UseExchange(string name);
		IRabbitMqQueueConfiguration RoutingKey(string routePattern);
		IRabbitMqQueueConfiguration AutoDelete(bool value);
		IRabbitMqQueueConfiguration IsExclusive(bool value);
		IRabbitMqQueueConfiguration OverwriteIfExists { get; }

		IRabbitMqQueueConfiguration DeclareQueue(Uri uri);
		IRabbitMqQueueConfiguration DeclareQueue(string uri);
	}
}