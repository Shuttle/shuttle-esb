using System;

namespace Shuttle.ESB.RabbitMq.Interfaces
{
	public interface IRabbitMqExchangeConfiguration
	{
		IRabbitMqExchangeConfiguration DefineExchange(string name);
		IRabbitMqExchangeConfiguration Type(string type);
		IRabbitMqExchangeConfiguration IsDurable(bool value);
		
		IRabbitMqQueueConfiguration DeclareQueue(Uri uri);
		IRabbitMqQueueConfiguration DeclareQueue(string uri);
	}
}