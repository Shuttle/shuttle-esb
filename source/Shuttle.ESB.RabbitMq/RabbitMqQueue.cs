using System;
using System.Collections.Generic;
using System.IO;
using System.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Transactions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMq
{
	public class RabbitMqQueue : IQueue, ICreate, IDrop, IPurge, ICount, IQueueReader
	{
		[ThreadStatic]
		private static object _underlyingMessageData;

		private readonly RabbitMqConnector _connector;
		private readonly ConfigurationItem<int> _localQueueTimeout;
		private readonly ConfigurationItem<int> _remoteQueueTimeout;
		private readonly RabbitMqQueuePath _queuePath;
		private readonly RabbitMqQueueConfiguration _configuration;
		private readonly ILog _log;

		// Todo: move to config
		private readonly TimeSpan _timeout;

		public RabbitMqQueue(RabbitMqConnector connector, RabbitMqQueuePath queuePath, RabbitMqQueueConfiguration configuration)
		{
			_configuration = configuration;
			_connector = connector;
			_queuePath = queuePath;
			_localQueueTimeout = ConfigurationItem<int>.ReadSetting("LocalQueueTimeout", 0);
			_remoteQueueTimeout = ConfigurationItem<int>.ReadSetting("RemoteQueueTimeout", 2000);
			_log = Log.For(this);

			IsLocal = queuePath.Host.Equals(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase);
			IsTransactional = _configuration.IsTransactional;

			Uri = queuePath.Uri;

			_timeout = IsLocal
						? TimeSpan.FromMilliseconds(_localQueueTimeout.GetValue())
						: TimeSpan.FromMilliseconds(_remoteQueueTimeout.GetValue());
		}

		private void EnlistToTransactionScope()
		{
			new RabbitMqResourceManager(Channel);
		}

		private IModel Channel
		{
			get { return _connector.RequestChannel(); }
		}

		public int Count
		{
			get
			{
				var result = Channel.QueueDeclare(_queuePath.QueueName, _configuration.IsDurable, false, false, null);
				return result == null ? -1 : (int)result.MessageCount;
			}
		}

		public void Create()
		{
			// no need to check if queue exists for the call is idempotent
			Channel.QueueDeclare(_queuePath.QueueName, _configuration.IsDurable, _configuration.IsExclusive, _configuration.AutoDelete, null);

			if (!string.IsNullOrEmpty(_configuration.Exchange))
			{
				Channel.QueueBind(_queuePath.QueueName, _configuration.Exchange, _queuePath.QueueName);
			}

			_log.Information(string.Format("Created private rabbitMq queue '{0}'.", Uri));
		}

		public void Drop()
		{
			if (!IsLocal)
			{
				throw new InvalidOperationException(string.Format(RabbitMqResources.CannotDropRemoteQueue, Uri));
			}

			Channel.QueueDelete(_queuePath.QueueName);
			_log.Information(string.Format("Dropped private rabbitMq queue '{0}'.", Uri));
		}

		public void Purge()
		{
			Channel.QueuePurge(_queuePath.QueueName);
			_log.Information(string.Format("Purged private rabbitMq queue '{0}'.", Uri));
		}

		public bool IsTransactional { get; private set; }

		public bool IsLocal { get; private set; }
		public Uri Uri { get; private set; }

		public QueueAvailability Exists()
		{
			return QueueAvailability.Unknown;
		}

		public bool IsEmpty()
		{
			return Count == 0;
		}

		public object UnderlyingMessageData
		{
			get { return _underlyingMessageData; }
		}

		private void BeginTransaction()
		{
			switch (TransactionType())
			{
				case MessageQueueTransactionType.Automatic:
					EnlistToTransactionScope();
					break;
				case MessageQueueTransactionType.Single:
					Channel.TxSelect();
					break;
			}
		}

		private void EndTransaction(BasicGetResult message = null)
		{
			if (TransactionType() == MessageQueueTransactionType.Single)
			{
				if (message != null)
				{
					Channel.BasicAck(message.DeliveryTag, false);
				}
				Channel.TxCommit();
			}
		}

		private void RollbackTransaction(BasicGetResult message = null)
		{
			if (TransactionType() == MessageQueueTransactionType.Single)
			{
				if (message != null)
				{
					Channel.BasicNack(message.DeliveryTag, false, true);
				}

				Channel.TxRollback();
			}
		}

		public void Enqueue(object data)
		{
			Guard.AgainstNull(data, "data");
			BeginTransaction();

			try
			{
				var formatter = new BinaryFormatter();
				var stream = new MemoryStream();
				formatter.Serialize(stream, data);

				var basicProperties = Channel.CreateBasicProperties();
				Channel.BasicPublish(_configuration.Exchange, _queuePath.QueueName, basicProperties, stream.ToBytes());

				EndTransaction();
			}
			catch
			{
				RollbackTransaction();
				throw;
			}
		}

		public void Enqueue(Guid messageId, Stream stream)
		{
			BeginTransaction();

			try
			{
				var basicProperties = Channel.CreateBasicProperties();
				basicProperties.MessageId = messageId.ToString();
				basicProperties.DeliveryMode = 2;
				Channel.BasicPublish(_configuration.Exchange, _queuePath.QueueName, basicProperties, stream.ToBytes());

				EndTransaction();
			}
			catch
			{
				RollbackTransaction();
				throw;
			}
		}

		public Stream Dequeue()
		{
			ResetUnderlyingMessageData();
			BeginTransaction();

			try
			{
				var message = Channel.BasicGet(_queuePath.QueueName, !IsTransactional);
				if (message != null)
				{
					_underlyingMessageData = new MemoryStream(message.Body);
					EndTransaction(message);

					return message.Body.Length == 0
									 ? null
									 : _underlyingMessageData as MemoryStream;
				}

				RollbackTransaction();
			}
			catch
			{
				RollbackTransaction();
				throw;
			}

			return null;
		}

		public Stream Dequeue(Guid messageId)
		{
			throw new NotSupportedException("Used only for memory queues.");
		}

		public bool Remove(Guid messageId)
		{
			try
			{
				using (var subscriber = new Subscription(Channel, _queuePath.QueueName))
				{
					BasicDeliverEventArgs message;
					while (subscriber.Next(100, out message))
					{
						if (message.BasicProperties.MessageId == messageId.ToString())
						{
							subscriber.Ack(message);
							return true;
						}
					}

					return false;
				}
			}
			catch (Exception e)
			{
				_log.Error(string.Format(RabbitMqResources.RemoveError, messageId, Uri, e.CompactMessages()));
				throw;
			}
		}

		public IEnumerable<Stream> Read(int top)
		{
			var messageList = new List<Stream>();

			using (var subscriber = new Subscription(Channel, _queuePath.QueueName))
			{
				BasicDeliverEventArgs message;
				while (subscriber.Next(100, out message))
				{
					_underlyingMessageData = new MemoryStream(message.Body);
					messageList.Add(_underlyingMessageData as Stream);
				}
			}

			return messageList;
		}

		private void ResetUnderlyingMessageData()
		{
			_underlyingMessageData = null;
		}

		private MessageQueueTransactionType TransactionType()
		{
			return IsTransactional
					? Transaction.Current != null
						? MessageQueueTransactionType.Automatic
						: MessageQueueTransactionType.Single
					: MessageQueueTransactionType.None;
		}
	}
}