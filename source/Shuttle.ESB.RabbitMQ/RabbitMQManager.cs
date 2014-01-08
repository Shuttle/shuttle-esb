using System;
using System.Collections.Generic;
using System.Threading;
using RabbitMQ.Client;

namespace Shuttle.ESB.RabbitMQ
{
	public class RabbitMQManager : IRabbitMqManager, IDisposable
	{
		private readonly Dictionary<string, IConnection> connections = new Dictionary<string, IConnection>();
		private readonly Dictionary<int, ConnectionModel> models = new Dictionary<int, ConnectionModel>();

		private readonly object padlock = new object();

		public IModel GetModel(RabbitMQQueue queue)
		{
			var key = Thread.CurrentThread.ManagedThreadId;

			if (models.ContainsKey(key))
			{
				return models[key].Model;
			}

			lock (padlock)
			{
				if (models.ContainsKey(key))
				{
					return models[key].Model;
				}

				var connectionKey = ConnectionKey(queue);

				models.Add(key, new ConnectionModel(connectionKey, GetConnection(connectionKey, queue).CreateModel()));

				return models[key].Model;
			}
		}

		private IConnection GetConnection(string connectionKey, RabbitMQQueue queue)
		{
			if (connections.ContainsKey(connectionKey))
			{
				return connections[connectionKey];
			}

			var factory = new ConnectionFactory
			{
				UserName = queue.Username,
				Password = queue.Password,
				HostName = queue.Host,
				VirtualHost = queue.VirtualHost,
				Port = queue.Port
			};

			var connection = factory.CreateConnection();

			connections.Add(connectionKey, connection);

			return connection;
		}

		private string ConnectionKey(RabbitMQQueue queue)
		{
			return string.Format("{0}{1}{2}", queue.HasUserInfo ? string.Format("{0}@", queue.Username) : string.Empty, string.Concat(queue.Host, ":", queue.Port), queue.VirtualHost);
		}

		public void Dispose()
		{
			foreach (var connectionModel in models.Values)
			{
				connectionModel.Model.Dispose();
			}

			foreach (var connection in connections.Values)
			{
				connection.Dispose();
			}
		}
	}
}