using System;
using System.IO;
using System.Linq;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.RabbitMQ
{
	public class RabbitMQQueue : IQueue, ICount
	{
		internal const string SCHEME = "rabbitmq";

		private readonly IRabbitMqManager _manager;

		public RabbitMQQueue(Uri uri, IRabbitMqManager manager)
		{
			Guard.AgainstNull(manager, "manager");

			_manager = manager;

			Password = "";
			Username = "";
			Host = uri.Host;
			Port = uri.Port;

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

			var builder = new UriBuilder(uri);

			if (Host.Equals("."))
			{
				builder.Host = Environment.MachineName.ToLower();
			}

			builder.Port = Port;
			builder.UserName = Username;
			builder.Password = Password;
			builder.Path = VirtualHost == "/" ? string.Format("/{0}", Queue) : string.Format("/{0}/{1}", VirtualHost, Queue);

			Uri = builder.Uri;

			IsLocal = Uri.Host.Equals(Environment.MachineName, StringComparison.OrdinalIgnoreCase);
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
				_manager.GetModel(this).QueueDeclarePassive(Queue);
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

		public object UnderlyingMessageData { get; private set; }

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

		public void Enqueue(object data)
		{
			throw new NotImplementedException();
		}

		public void Enqueue(Guid messageId, Stream stream)
		{
			Guard.AgainstNull(messageId, "messageId");
			Guard.AgainstNull(stream, "stream");

			var model = _manager.GetModel(this);

			var properties = model.CreateBasicProperties();

			properties.SetPersistent(true);
			properties.CorrelationId = messageId.ToString();

			model.BasicPublish("", Queue, false, false, properties, stream.ToBytes());
		}

		public Stream Dequeue()
		{
			var model = _manager.GetModel(this);

			using (var subscription = new Subscription(model, Queue, false))
			{
				BasicDeliverEventArgs result;

				if (subscription.Next(100, out result))
				{
					return new MemoryStream(result.Body);
				}
			}

			return null;
		}

		public Stream Dequeue(Guid messageId)
		{
			var model = _manager.GetModel(this);

			using (var subscription = new Subscription(model, Queue, false))
			{
				var read = true;

				while (read)
				{
					BasicDeliverEventArgs result;

					if (subscription.Next(100, out result))
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
			}

			return null;
		}

		public bool Remove(Guid messageId)
		{
			var model = _manager.GetModel(this);

			using (var subscription = new Subscription(model, Queue, false))
			{
				var read = true;

				while (read)
				{
					BasicDeliverEventArgs result;

					if (subscription.Next(100, out result))
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
							subscription.Ack(result);

							return true;
						}
					}
					else
					{
						read = false;
					}
				}
			}

			return false;
		}

		public int Count
		{
			get
			{
				try
				{
					return (int) _manager.GetModel(this).QueueDeclarePassive(Queue).MessageCount;
				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}
	}
}