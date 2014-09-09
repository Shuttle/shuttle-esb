using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class QueueManager : IQueueManager, IDisposable
	{
		private bool _initialized;
		private bool _initializing;
		private static readonly object _padlock = new object();

		private readonly List<IQueue> _queues = new List<IQueue>();
		private readonly List<IQueueFactory> _queueFactories = new List<IQueueFactory>();

		private readonly ILog _log;

		public QueueManager()
		{
			UriResolver = new DefaultUriResolver();

			_log = Log.For(this);
		}

		private List<IQueueFactory> QueueFactories()
		{
			if (_initialized || _initializing)
			{
				return _queueFactories;
			}

			try
			{
				_initializing = true;

				lock (_padlock)
				{
					if (!_initialized)
					{
						var factoryTypes = new List<Type>();
						var scan = true;

						if (ServiceBusConfiguration.ServiceBusSection != null && ServiceBusConfiguration.ServiceBusSection.QueueFactories != null)
						{
							scan = ServiceBusConfiguration.ServiceBusSection.QueueFactories.Scan;

							foreach (QueueFactoryElement queueFactoryElement in ServiceBusConfiguration.ServiceBusSection.QueueFactories)
							{
								factoryTypes.Add(Type.GetType(queueFactoryElement.Type));
							}
						}

						if (scan)
						{
							factoryTypes.AddRange(new ReflectionService().GetTypes<IQueueFactory>());
						}

						foreach (var type in factoryTypes)
						{
							try
							{
								type.AssertDefaultConstructor(string.Format(ESBResources.DefaultConstructorRequired, "Queue factory", type.FullName));

								var instance = (IQueueFactory)Activator.CreateInstance(type);

								if (!ContainsQueueFactory(instance.Scheme))
								{
									RegisterQueueFactory(instance);
								}
							}
							catch (Exception ex)
							{
								_log.Warning(string.Format("Queue factory not instantiated: {0}", ex.Message));
							}
						}

						_initialized = true;
					}
				}
			}
			finally
			{
				_initializing = false;
			}

			return _queueFactories;
		}

		public IQueueFactory GetQueueFactory(string scheme)
		{
			Uri uri;

			return Uri.TryCreate(scheme, UriKind.Absolute, out uri)
					   ? GetQueueFactory(uri)
					   : QueueFactories().Find(factory => factory.Scheme.Equals(scheme, StringComparison.InvariantCultureIgnoreCase));
		}

		public IQueueFactory GetQueueFactory(Uri uri)
		{
			foreach (var factory in QueueFactories().Where(factory => factory.CanCreate(uri)))
			{
				return factory;
			}

			throw new QueueFactoryNotFoundException(uri.Scheme);
		}

		public IQueue GetQueue(string uri)
		{
			var queue =
				_queues.Find(
					candidate => Find(candidate, uri));

			if (queue != null)
			{
				return queue;
			}

			lock (_padlock)
			{
				queue =
					_queues.Find(
						candidate => Find(candidate, uri));

				if (queue != null)
				{
					return queue;
				}

				var queueUri = new Uri(uri);

				if (queueUri.Scheme.Equals("resolver"))
				{
					if (UriResolver == null)
					{
						throw new InvalidOperationException(string.Format(ESBResources.NoUriResolverException, uri));
					}

					var resolvedQueueUri = UriResolver.Get(uri);

					if (resolvedQueueUri == null)
					{
						throw new KeyNotFoundException(string.Format(ESBResources.UriNameNotFoundException, UriResolver.GetType().FullName, uri));
					}

					queue = new ResolvedQueue(GetQueueFactory(resolvedQueueUri).Create(resolvedQueueUri), queueUri);
				}
				else
				{
					queue = GetQueueFactory(queueUri).Create(queueUri);
				}

				_queues.Add(queue);

				return queue;
			}
		}

		private static bool Find(IQueue candidate, string uri)
		{
			return candidate.Uri.ToString().Equals(uri, StringComparison.InvariantCultureIgnoreCase);
		}

		public IQueue CreateQueue(string uri)
		{
			return CreateQueue(new Uri(uri));
		}

		public IQueue CreateQueue(Uri uri)
		{
			return GetQueueFactory(uri).Create(uri);
		}

		public void CreatePhysicalQueues(IServiceBusConfiguration serviceBusConfiguration)
		{
			if (serviceBusConfiguration.HasInbox)
			{
				CreateQueues(serviceBusConfiguration.Inbox);

				if (serviceBusConfiguration.HasDeferredQueue)
				{
					serviceBusConfiguration.Inbox.DeferredQueue.AttemptCreate();
				}
			}

			if (serviceBusConfiguration.HasOutbox)
			{
				CreateQueues(serviceBusConfiguration.Outbox);
			}

			if (serviceBusConfiguration.HasControlInbox)
			{
				CreateQueues(serviceBusConfiguration.ControlInbox);
			}

			if (serviceBusConfiguration.IsWorker)
			{
				serviceBusConfiguration.Worker.DistributorControlInboxWorkQueue.AttemptCreate();
			}
		}

		public IEnumerable<IQueueFactory> GetQueueFactories()
		{
			return new ReadOnlyCollection<IQueueFactory>(_queueFactories);
		}

		private void CreateQueues(IWorkQueueConfiguration workQueueConfiguration)
		{
			workQueueConfiguration.WorkQueue.AttemptCreate();

			var errorQueueConfiguration = workQueueConfiguration as IErrorQueueConfiguration;

			if (errorQueueConfiguration != null)
			{
				errorQueueConfiguration.ErrorQueue.AttemptCreate();
			}
		}

		public void RegisterQueueFactory(IQueueFactory queueFactory)
		{
			Guard.AgainstNull(queueFactory, "queueFactory");

			var factory = GetQueueFactory(queueFactory.Scheme);

			if (factory != null)
			{
				QueueFactories().Remove(factory);

				_log.Warning(string.Format(ESBResources.DuplicateQueueFactoryReplaced, queueFactory.Scheme, factory.GetType().FullName, queueFactory.GetType().FullName));
			}

			QueueFactories().Add(queueFactory);
		}

		public bool ContainsQueueFactory(string scheme)
		{
			return GetQueueFactory(scheme) != null;
		}

		public IUriResolver UriResolver { get; set; }

		public void Dispose()
		{
			_queueFactories.AttemptDispose();
			_queues.AttemptDispose();

			_queueFactories.Clear();
			_queues.Clear();

			_initialized = false;
		}
	}
}