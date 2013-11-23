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
		private readonly ILog _log;

		// Todo: move to config
		private readonly TimeSpan _timeout;

		public RabbitMqQueue(RabbitMqConnector connector)
		{
			_connector = connector;
			_localQueueTimeout = ConfigurationItem<int>.ReadSetting("LocalQueueTimeout", 0);
			_remoteQueueTimeout = ConfigurationItem<int>.ReadSetting("RemoteQueueTimeout", 2000);
			_log = Log.For(this);

			IsLocal = connector.QueuePath.Host.Equals(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase);
			IsTransactional = connector.QueueConfiguration.IsTransactional;

			Uri = connector.QueuePath.Uri;

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
				var result = Channel.QueueDeclare(_connector.QueuePath.QueueName, _connector.QueueConfiguration.IsDurable, false, false, null);
				return result == null ? -1 : (int)result.MessageCount;
			}
		}

		public void Create()
		{
			if (_connector.QueueConfiguration.OverwriteIfExists)
			{
				try
				{
					Drop();
				}
				catch (Exception e)
				{
					throw;
				}
			}

			// no need to check if queue exists for the call is idempotent
			Channel.QueueDeclare(
				_connector.QueuePath.QueueName,
				_connector.QueueConfiguration.IsDurable, 
				_connector.QueueConfiguration.IsExclusive, 
				_connector.QueueConfiguration.AutoDelete, null);

			if (!string.IsNullOrEmpty(_connector.QueueConfiguration.Exchange))
			{
				Channel.QueueBind(
					_connector.QueuePath.QueueName, 
					_connector.QueueConfiguration.Exchange, 
					_connector.QueueConfiguration.RoutingKey ?? _connector.QueuePath.QueueName);
			}

			_log.Information(string.Format("Created rabbitMq queue '{0}'.", Uri));
		}

		public void Drop()
		{
			if (!IsLocal)
			{
				throw new InvalidOperationException(string.Format(RabbitMqResources.CannotDropRemoteQueue, Uri));
			}

			Channel.QueueDelete(_connector.QueuePath.QueueName);
			_log.Information(string.Format("Dropped rabbitMq queue '{0}'.", Uri));
		}

		public void Purge()
		{
			Channel.QueuePurge(_connector.QueuePath.QueueName);
			_log.Information(string.Format("Purged rabbitMq queue '{0}'.", Uri));
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
				Channel.BasicPublish(_connector.QueueConfiguration.Exchange, _connector.QueuePath.QueueName, basicProperties, stream.ToBytes());

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
				basicProperties.DeliveryMode = (byte)(_connector.QueueConfiguration.IsDurable ? 1 : 0);

				Channel.BasicPublish(_connector.QueueConfiguration.Exchange, _connector.QueuePath.QueueName, basicProperties, stream.ToBytes());

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
				var message = Channel.BasicGet(_connector.QueuePath.QueueName, !IsTransactional);
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
				using (var subscriber = new Subscription(Channel, _connector.QueuePath.QueueName))
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

			using (var subscriber = new Subscription(Channel, _connector.QueuePath.QueueName))
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