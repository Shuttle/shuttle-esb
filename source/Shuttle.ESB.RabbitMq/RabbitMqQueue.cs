using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Text.RegularExpressions;
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

		private readonly IModel _channel;
		private readonly ConfigurationItem<int> _localQueueTimeout;
		private readonly ConfigurationItem<int> _remoteQueueTimeout;
		private readonly TimeSpan _timeout;
		private readonly RabbitMqQueuePath _queuePath;
		private readonly ILog _log;

		public RabbitMqQueue(IModel channel, RabbitMqQueuePath queuePath, bool isTransactional)
		{
			_channel = channel;
			_queuePath = queuePath;
			_localQueueTimeout = ConfigurationItem<int>.ReadSetting("LocalQueueTimeout", 0);
			_remoteQueueTimeout = ConfigurationItem<int>.ReadSetting("RemoteQueueTimeout", 2000);
			_log = Log.For(this);

			IsLocal = queuePath.Host.Equals(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase);
			IsTransactional = isTransactional;

			Uri = queuePath.Uri;

			_timeout = IsLocal
						? TimeSpan.FromMilliseconds(_localQueueTimeout.GetValue())
						: TimeSpan.FromMilliseconds(_remoteQueueTimeout.GetValue());
		}

		private void EnlistToTransactionScope()
		{
			new RabbitMqResourceManager(_channel);
		}

		public int Count
		{
			get
			{
				var result = _channel.QueueDeclare(_queuePath.QueueName, true, false, false, null);
				return result == null ? -1 : (int)result.MessageCount;
			}
		}

		public void Create()
		{
			// no need to check if queue exists for the call is idempotent
			_channel.QueueDeclare(_queuePath.QueueName, true, false, false, null);

			if (!string.IsNullOrEmpty(_queuePath.Exchange))
				_channel.QueueBind(_queuePath.QueueName, _queuePath.Exchange, _queuePath.QueueName);

			_log.Information(string.Format("Created private rabbitMq queue '{0}'.", Uri));
		}

		public void Drop()
		{
			if (!IsLocal)
				throw new InvalidOperationException(string.Format(RabbitMqResources.CannotDropRemoteQueue, Uri));

			_channel.QueueDelete(_queuePath.QueueName);
			_log.Information(string.Format("Dropped private msmq queue '{0}'.", Uri));
		}

		public void Purge()
		{
			_channel.QueuePurge(_queuePath.QueueName);
			_log.Information(string.Format("Purged private msmq queue '{0}'.", Uri));
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
					_channel.TxSelect();
					break;
			}
		}

		private void EndTransaction()
		{
			if (TransactionType() == MessageQueueTransactionType.Single)
				_channel.TxCommit();
		}

		private void RollbackTransaction()
		{
			if (TransactionType() == MessageQueueTransactionType.Single)
				_channel.TxRollback();
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

				var basicProperties = _channel.CreateBasicProperties();
				_channel.BasicPublish(_queuePath.Exchange, _queuePath.QueueName, basicProperties, stream.ToBytes());

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
				var basicProperties = _channel.CreateBasicProperties();
				basicProperties.MessageId = messageId.ToString();
				basicProperties.DeliveryMode = 2;
				_channel.BasicPublish(_queuePath.Exchange, _queuePath.QueueName, basicProperties, stream.ToBytes());

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
				var message = _channel.BasicGet(_queuePath.QueueName, false);
				if (message != null)
				{
					_underlyingMessageData = new MemoryStream(message.Body);
					_channel.BasicAck(message.DeliveryTag, false);

					EndTransaction();

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
				using (var subscriber = new Subscription(_channel, _queuePath.QueueName))
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

			using (var subscriber = new Subscription(_channel, _queuePath.QueueName))
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