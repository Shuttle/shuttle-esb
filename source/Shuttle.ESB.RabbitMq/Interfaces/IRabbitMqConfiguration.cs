namespace Shuttle.ESB.RabbitMq.Interfaces
{
	public interface IRabbitMqConfiguration
	{
		IRabbitMqExchangeConfiguration DefineExchange(string name);
	}
}