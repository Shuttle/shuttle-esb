using RabbitMQ.Client;

namespace Shuttle.ESB.RabbitMQ
{
	public interface IRabbitMqManager
	{
		IModel GetModel(RabbitMQQueue queue);
	}
}