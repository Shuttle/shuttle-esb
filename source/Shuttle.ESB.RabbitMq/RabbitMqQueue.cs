using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMQ
{
	public class RabbitMQQueue : IQueue, ICount, ICreate, IDrop, IDisposable, IAcknowledge, IPurge
	{
		internal const string SCHEME = "rabbitmq";

		private readonly object connectionlock = new object();
		private readonly object queuelock = new object();
		private readonly object disposelock = new object();

		private readonly ConnectionFactory _factory;
		private IConnection _connection;

		private readonly Dictionary<int, Channel> _channels = new Dictionary<int, Channel>();

		public RabbitMQQueue(Uri uri)
		{
			Guard.AgainstNull(uri, "uri");

			Password = "";
			Username = "";
			Port = uri.Port;
			Host = uri.Host;

			if (uri.UserInfo.Contains(':'))
			{
				Username = uri.UserInfo.Split(':').First();
				Password = uri.UserInfo.Split(':').Last();
			}

			switch (uri.Segments.Length)
			{
				case 2:
					{
						VirtualHost = "/";
						Queue = uri.Segments[1];
						break;
					}
				case 3:
					{
						VirtualHost = uri.Segments[1];
						Queue = uri.Segments[2];
						break;
					}
				default:
					{
						throw new UriFormatException(string.Format(ESBResources.UriFormatException,
																   "rabbitmq://[username:password@]host:port/[vhost/]queue", Uri));
					}
			}

			if (Host.Equals("."))
			{
				Host = "localhost";
			}

			var builder = new UriBuilder(uri)
				{
					Host = Host,
					Port = Port,
					UserName = Username,
					Password = Password,
					Path = VirtualHost == "/" ? string.Format("/{0}", Queue) : string.Format("/{0}/{1}", VirtualHost, Queue)
				};

			Uri = builder.Uri;

			IsLocal = Uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) || Uri.Host.Equals("127.0.0.1");

			_factory = new ConnectionFactory
				{
					UserName = Username,
					Password = Password,
					HostName = Host,
					VirtualHost = VirtualHost,
					Port = Port,
					RequestedHeartbeat = 30 //TODO: get from configuration
				};
		}

		public bool IsLocal { get; private set; }

		public bool IsTransactional
		{
			get { return false; }
		}

		public Uri Uri { get; private set; }

		public QueueAvailability Exists()
		{
			try
			{
				AccessQueue(() => { GetChannel().Model.QueueDeclarePassive(Queue); });
			}
			catch
			{
				return QueueAvailability.Missing;
			}

			return QueueAvailability.Exists;
		}

		public bool IsEmpty()
		{
			return Count == 0;
		}

		public string Username { get; private set; }
		public string Password { get; private set; }
		public string Host { get; private set; }
		public int Port { get; private set; }
		public string VirtualHost { get; private set; }
		public string Queue { get; private set; }

		public bool HasUserInfo
		{
			get { return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password); }
		}

		public void Enqueue(Guid messageId, Stream stream)
		{
			Guard.AgainstNull(messageId, "messageId");
			Guard.AgainstNull(stream, "stream");

			AccessQueue(() =>
				{
					var model = GetChannel().Model;

					var properties = model.CreateBasicProperties();

					properties.SetPersistent(true);
					properties.CorrelationId = messageId.ToString();

					model.BasicPublish("", Queue, false, false, properties, stream.ToBytes());
				});
		}

		public Stream Dequeue()
		{
			return AccessQueue<Stream>(() =>
				{
					var result = GetChannel().Next();

					return (result != null)
							   ? new MemoryStream(result.Body)
							   : null;
				});
		}

		public Stream Dequeue(Guid messageId)
		{
			return AccessQueue<Stream>(() =>
				{
					var read = true;

					while (read)
					{
						var result = GetChannel().Next();

						if (result != null)
						{
							Guid guid;

							try
							{
								guid = new Guid(result.BasicProperties.CorrelationId);
							}
							catch
							{
								guid = Guid.Empty;
							}

							if (guid.Equals(messageId))
							{
								return new MemoryStream(result.Body);
							}
						}
						else
						{
							read = false;
						}
					}

					return null;
				});
		}

		public bool Remove(Guid messageId)
		{
			return AccessQueue(() =>
				{
					var read = true;

					while (read)
					{
						var result = GetChannel().Next();

						if (result != null)
						{
							Guid guid;

							try
							{
								guid = new Guid(result.BasicProperties.CorrelationId);
							}
							catch
							{
								guid = Guid.Empty;
							}

							if (guid.Equals(messageId))
							{
								GetChannel().Acknowledge();

								return true;
							}
						}
						else
						{
							read = false;
						}
					}

					return false;
				});
		}

		public int Count
		{
			get { return AccessQueue(() => (int)QueueDeclare(GetChannel().Model).MessageCount); }
		}

		public void Drop()
		{
			AccessQueue(() => { GetChannel().Model.QueueDelete(Queue); });
		}

		public void Create()
		{
			AccessQueue(() => { QueueDeclare(GetChannel().Model); });
		}

		private QueueDeclareOk QueueDeclare(IModel model)
		{
			return model.QueueDeclare(Queue, true, false, false, null);
		}

		private IConnection GetConnection()
		{
			if (_connection != null)
			{
				return _connection;
			}

			lock (connectionlock)
			{
				if (_connection == null)
				{
					_connection = _factory.CreateConnection();

					_connection.AutoClose = false;
				}
			}

			return _connection;
		}

		private Channel GetChannel()
		{
			var key = Thread.CurrentThread.ManagedThreadId;

			if (_channels.ContainsKey(key))
			{
				return _channels[key];
			}

			lock (queuelock)
			{
				if (_channels.ContainsKey(key))
				{
					return _channels[key];
				}

				var retry = 0;
				IConnection connection = null;

				while (connection == null && retry < 3) // TODO: retry should be configurable
				{
					try
					{
						connection = GetConnection();
					}
					catch (Exception)
					{
						retry++;
					}
				}

				if (connection == null)
				{
					throw new ConnectionException(); // TODO: proper message maybe
				}

				var model = connection.CreateModel();

				model.BasicQos(0, 1, false);

				QueueDeclare(model);

				var channel = new Channel(model, new Subscription(model, Queue, false), 100); // TODO: timeout in configuration (local & remote)

				_channels.Add(key, channel);

				return channel;
			}
		}

		public void Dispose()
		{
			lock (disposelock)
			{
				foreach (var channel in _channels.Values)
				{
					if (channel.Model.IsOpen)
					{
						channel.Model.Close();
					}
				}
				
				_channels.Values.AttemptDispose();
				_channels.Clear();

				if (_connection != null)
				{
					if (_connection.IsOpen)
					{
						_connection.Close(1000); // TODO: timeout in configuration
					}

					try
					{
						_connection.Dispose();
					}
					catch (IOException)
					{
					}

					_connection = null;
				}
			}
		}

		public void Acknowledge()
		{
			AccessQueue(() => GetChannel().Acknowledge());
		}

		public void Purge()
		{
			AccessQueue(() =>
				{
					GetChannel().Model.QueuePurge(Queue);
				});
		}

		private void AccessQueue(Action action, int retry = 0)
		{
			try
			{
				action.Invoke();
			}
			catch (ConnectionException)
			{
				if (retry == 3)
				{
					throw;
				}

				Dispose();

				AccessQueue(action, retry + 1);
			}
		}

		private T AccessQueue<T>(Func<T> action, int retry = 0)
		{
			try
			{
				return action.Invoke();
			}
			catch (ConnectionException)
			{
				if (retry == 3)
				{
					throw;
				}

				Dispose();

				return AccessQueue(action, retry + 1);
			}
		}
	}
}