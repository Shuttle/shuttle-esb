using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMQ
{
	public class RabbitMQQueue : IQueue, ICount, ICreate, IDrop, IDisposable, IAcknowledge, IPurge
	{
		private readonly int _timeout;
		private readonly IRabbitMQConfiguration _configuration;

		private readonly object connectionlock = new object();
		private readonly object queuelock = new object();
		private readonly object disposelock = new object();
		private readonly int _operationRetryCount;

		private readonly RabbitMQUriParser parser;

		private readonly ConnectionFactory _factory;
		private IConnection _connection;

		private readonly Dictionary<int, Channel> _channels = new Dictionary<int, Channel>();

		public RabbitMQQueue(Uri uri, IRabbitMQConfiguration configuration)
		{
			Guard.AgainstNull(uri, "uri");
			Guard.AgainstNull(configuration, "configuration");

			parser = new RabbitMQUriParser(uri);

			Uri = parser.Uri;

			_configuration = configuration;

			_timeout = parser.Local
				           ? configuration.LocalQueueTimeoutMilliseconds
				           : configuration.RemoteQueueTimeoutMilliseconds;

			_operationRetryCount = _configuration.OperationRetryCount;

			if (_operationRetryCount < 1)
			{
				_operationRetryCount = 3;
			}

			_factory = new ConnectionFactory
				{
					UserName = parser.Username,
					Password = parser.Password,
					HostName = parser.Host,
					VirtualHost = parser.VirtualHost,
					Port = parser.Port,
					RequestedHeartbeat = configuration.RequestedHeartbeat
				};
		}

		public Uri Uri { get; private set; }

		public bool IsEmpty()
		{
			return Count == 0;
		}

		public bool HasUserInfo
		{
			get { return !string.IsNullOrEmpty(parser.Username) && !string.IsNullOrEmpty(parser.Password); }
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

					model.BasicPublish("", parser.Queue, false, false, properties, stream.ToBytes());
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
			get { return AccessQueue(() => (int) QueueDeclare(GetChannel().Model).MessageCount); }
		}

		public void Drop()
		{
			AccessQueue(() => { GetChannel().Model.QueueDelete(parser.Queue); });
		}

		public void Create()
		{
			AccessQueue(() => { QueueDeclare(GetChannel().Model); });
		}

		private QueueDeclareOk QueueDeclare(IModel model)
		{
			return model.QueueDeclare(parser.Queue, true, false, false, null);
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

				while (connection == null && retry < _operationRetryCount)
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
					throw new ConnectionException(string.Format(RabbitMQResources.ConnectionException, Uri.Secured()));
				}

				var model = connection.CreateModel();

				model.BasicQos(0, 1, false);

				QueueDeclare(model);

				var channel = new Channel(model, new Subscription(model, parser.Queue, false), _timeout);

				_channels.Add(key, channel);

				return channel;
			}
		}

		public void Dispose()
		{
			lock (disposelock)
			{
				_channels.Values.AttemptDispose();
				_channels.Clear();

				if (_connection != null)
				{
					if (_connection.IsOpen)
					{
						_connection.Close(_configuration.ConnectionCloseTimeoutMilliseconds);
					}

					try
					{
						_connection.Dispose();
					}
					catch (IOException)
					{
					}
					catch (Exception ex)
					{
						var e = ex;
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
			AccessQueue(() => { GetChannel().Model.QueuePurge(parser.Queue); });
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