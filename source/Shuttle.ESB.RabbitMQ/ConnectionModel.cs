using RabbitMQ.Client;

namespace Shuttle.ESB.RabbitMQ
{
	internal class ConnectionModel
	{
		public ConnectionModel(string connectionKey, IModel model)
		{
			ConnectionKey = connectionKey;
			Model = model;
		}

		public string ConnectionKey { get; private set; }
		public IModel Model { get; private set; }
	}
}