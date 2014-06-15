using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMQ
{
	public class RabbitMQQueue : IQueue, ICreateQueue, IDropQueue, IDisposable, IPurgeQueue
	{
		private readonly IRabbitMQConfiguration _configuration;

		private readonly object _connectionLock = new object();
		private readonly object _queueLock = new object();
		private readonly object _disposeLock = new object();
		private readonly int _operationRetryCount;

		private readonly RabbitMQUriParser _parser;

		private readonly ConnectionFactory _factory;
		private IConnection _connection;

		private readonly Dictionary<int, Channel> _channels = new Dictionary<int, Channel>();

		public RabbitMQQueue(Uri uri, IRabbitMQConfiguration configuration)
		{
			Guard.AgainstNull(uri, "uri");
			Guard.AgainstNull(configuration, "configuration");

			_parser = new RabbitMQUriParser(uri);

			Uri = _parser.Uri;

			_configuration = configuration;

			_operationRetryCount = _configuration.OperationRetryCount;

			if (_operationRetryCount < 1)
			{
				_operationRetryCount = 3;
			}

			_factory = new ConnectionFactory
				{
					UserName = _parser.Username,
					Password = _parser.Password,
					HostName = _parser.Host,
					VirtualHost = _parser.VirtualHost,
					Port = _parser.Port,
					RequestedHeartbeat = configuration.RequestedHeartbeat
				};
		}

		public Uri Uri { get; private set; }

		public bool IsEmpty()
		{
			return AccessQueue(() =>
			{
				var result = GetChannel().Model.BasicGet(_parser.Queue, false);

				if (result == null)
				{
					return true;
				}

				GetChannel().Model.BasicReject(result.DeliveryTag, true);

				return false;
			});
		}

		public bool HasUserInfo
		{
			get { return !string.IsNullOrEmpty(_parser.Username) && !string.IsNullOrEmpty(_parser.Password); }
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

					model.BasicPublish("", _parser.Queue, false, false, properties, stream.ToBytes());
				});
		}

		public ReceivedMessage GetMessage()
		{
			return AccessQueue(() =>
				{
					var result = GetChannel().Next();

					if (result == null)
					{
						return null;
					}

					return new ReceivedMessage(new MemoryStream(result.Body), result);
				});
		}

		public void Drop()
		{
			AccessQueue(() => { GetChannel().Model.QueueDelete(_parser.Queue); });
		}

		public void Create()
		{
			AccessQueue(() => { QueueDeclare(GetChannel().Model); });
		}

		private QueueDeclareOk QueueDeclare(IModel model)
		{
			return model.QueueDeclare(_parser.Queue, true, false, false, null);
		}

		private IConnection GetConnection()
		{
			if (_connection != null)
			{
				return _connection;
			}

			lock (_connectionLock)
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

			lock (_queueLock)
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

				model.BasicQos(0, _parser.PrefetchCount == 0 ? _configuration.DefaultPrefetchCount : _parser.PrefetchCount, false);

				QueueDeclare(model);

				var channel = new Channel(model, _parser, _configuration);

				_channels.Add(key, channel);

				return channel;
			}
		}

		public void Dispose()
		{
			lock (_disposeLock)
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

		public void Acknowledge(object acknowledgementToken)
		{
			AccessQueue(() => GetChannel().Acknowledge((BasicDeliverEventArgs)acknowledgementToken));
		}

		public void Release(object acknowledgementToken)
		{
			AccessQueue(() =>
				{
					var basicDeliverEventArgs = (BasicDeliverEventArgs)acknowledgementToken;

					GetChannel().Model.BasicPublish("", _parser.Queue, false, false, basicDeliverEventArgs.BasicProperties, basicDeliverEventArgs.Body);
					GetChannel().Acknowledge(basicDeliverEventArgs);
				});
		}

		public void Purge()
		{
			AccessQueue(() => { GetChannel().Model.QueuePurge(_parser.Queue); });
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