using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMQ
{
	public class RabbitMQQueue : IQueue, ICount, ICreate, IDrop, IDisposable, IPurge
	{
		private class UnacknowledgedMessage
		{
			public UnacknowledgedMessage(Guid messageId, BasicDeliverEventArgs basicDeliverEventArgs)
			{
				BasicDeliverEventArgs = basicDeliverEventArgs;
				MessageId = messageId;
			}

			public BasicDeliverEventArgs BasicDeliverEventArgs { get; private set; }
			public Guid MessageId { get; private set; }
		}

		private readonly int _timeout;
		private readonly IRabbitMQConfiguration _configuration;
		private readonly List<UnacknowledgedMessage> _unacknowledgedMessages = new List<UnacknowledgedMessage>();

		private readonly object _connectionLock = new object();
		private readonly object _queueLock = new object();
		private readonly object _disposeLock = new object();
		private readonly object _unacknowledgedMessageLock = new object();
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

			_timeout = _parser.Local
				           ? configuration.LocalQueueTimeoutMilliseconds
				           : configuration.RemoteQueueTimeoutMilliseconds;

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
			return Count == 0;
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

		public Stream Dequeue()
		{
			return AccessQueue<Stream>(() =>
				{
					var result = GetChannel().Next();

					if (result == null)
					{
						return null;
					}

					lock (_unacknowledgedMessageLock)
					{
						_unacknowledgedMessages.Add(new UnacknowledgedMessage(new Guid(result.BasicProperties.CorrelationId), result));
					}

					return new MemoryStream(result.Body);
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
								lock (_unacknowledgedMessageLock)
								{
									_unacknowledgedMessages.Add(new UnacknowledgedMessage(guid, result));
								}

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

		public int Count
		{
			get { return AccessQueue(() => (int) QueueDeclare(GetChannel().Model).MessageCount); }
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

				model.BasicQos(0, 1, false);

				QueueDeclare(model);

				var channel = new Channel(model, new Subscription(model, _parser.Queue, false), _timeout);

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

		public void Acknowledge(Guid messageId)
		{
			AccessQueue(() =>
				{
					UnacknowledgedMessage unacknowledgedMessage;

					lock (_unacknowledgedMessageLock)
					{
						unacknowledgedMessage = _unacknowledgedMessages.Find(candidate => candidate.MessageId.Equals(messageId));
					}

					if (unacknowledgedMessage != null)
					{
						GetChannel().Acknowledge(unacknowledgedMessage.BasicDeliverEventArgs);
					}

					lock (_unacknowledgedMessageLock)
					{
						_unacknowledgedMessages.RemoveAll(candidate => candidate.MessageId.Equals(messageId));
					}
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