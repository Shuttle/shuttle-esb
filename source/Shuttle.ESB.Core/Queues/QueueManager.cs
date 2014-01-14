using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class QueueManager : IRequireInitialization, IQueueManager, IDisposable
	{
		private readonly IReflectionService _reflectionService;
		private static readonly object _padlock = new object();

		private readonly List<IQueue> _queues = new List<IQueue>();
		private readonly List<IQueueFactory> _queueFactories = new List<IQueueFactory>();
		private bool _initialized;

		private readonly ILog _log;

		public QueueManager(IReflectionService reflectionService)
		{
			Guard.AgainstNull(reflectionService, "reflectionService");

			_reflectionService = reflectionService;

			_log = Log.For(this);
		}

		public static IQueueManager Default()
		{
			return new QueueManager(new ReflectionService());
		}

		public IQueueFactory GetQueueFactory(string uri)
		{
			return GetQueueFactory(new Uri(uri));
		}

		public IQueueFactory GetQueueFactory(Uri uri)
		{
			foreach (var factory in QueueFactories().Where(factory => factory.CanCreate(uri)))
			{
				return factory;
			}

			throw new QueueFactoryNotFoundException(uri.Scheme);
		}

		private List<IQueueFactory> QueueFactories()
		{
			if (_initialized)
			{
				return _queueFactories;
			}

			lock (_padlock)
			{
				if (!_initialized)
				{
					var factoryTypes = new List<Type>();

					_reflectionService.GetAssemblies(AppDomain.CurrentDomain.BaseDirectory).ForEach(
						assembly => factoryTypes.AddRange(_reflectionService.GetTypes<IQueueFactory>(assembly)));

					foreach (var type in factoryTypes.Union(_reflectionService.GetTypes<IQueueFactory>()))
					{
						try
						{
							type.AssertDefaultConstructor(string.Format(ESBResources.DefaultConstructorRequired,
																		"Queue factory", type.FullName));

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

			return _queueFactories;
		}

		public IQueue GetQueue(string uri)
		{
			var queue =
				_queues.Find(
					candidate => FindByString(candidate, uri));

			if (queue != null)
			{
				return queue;
			}

			lock (_padlock)
			{
				queue =
					_queues.Find(
						candidate => FindByString(candidate, uri));

				if (queue != null)
				{
					return queue;
				}

				var identifier = new Uri(uri);

				queue = GetQueueFactory(identifier).Create(identifier);

				_queues.Add(queue);

				return queue;
			}
		}

		private static bool FindByString(IQueue candidate, string uri)
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

		public void CreatePhysicalQueues(IServiceBusConfiguration serviceBusConfiguration, QueueCreationType queueCreationType)
		{
			if (queueCreationType == QueueCreationType.None)
			{
				return;
			}

			if (serviceBusConfiguration.HasInbox)
			{
				CreateQueues(queueCreationType, serviceBusConfiguration.Inbox);
			}

			if (serviceBusConfiguration.HasOutbox)
			{
				CreateQueues(queueCreationType, serviceBusConfiguration.Outbox);
			}

			if (serviceBusConfiguration.HasControlInbox)
			{
				CreateQueues(queueCreationType, serviceBusConfiguration.ControlInbox);
			}

			if (serviceBusConfiguration.IsWorker)
			{
				if (ShouldCreate(queueCreationType, serviceBusConfiguration.Worker.DistributorControlInboxWorkQueue))
				{
					serviceBusConfiguration.Worker.DistributorControlInboxWorkQueue.AttemptCreate();
				}
			}
		}

		public IEnumerable<IQueueFactory> GetQueueFactories()
		{
			return new ReadOnlyCollection<IQueueFactory>(QueueFactories());
		}

		private void CreateQueues(QueueCreationType queueCreationType, IWorkQueueConfiguration workQueueConfiguration)
		{
			if (ShouldCreate(queueCreationType, workQueueConfiguration.WorkQueue))
			{
				workQueueConfiguration.WorkQueue.AttemptCreate();
			}

			var errorQueueConfiguration = workQueueConfiguration as IErrorQueueConfiguration;

			if (errorQueueConfiguration != null)
			{
				if (ShouldCreate(queueCreationType, errorQueueConfiguration.ErrorQueue))
				{
					errorQueueConfiguration.ErrorQueue.AttemptCreate();
				}
			}

			var journalQueueConfiguration = workQueueConfiguration as IJournalQueueConfiguration;

			if (journalQueueConfiguration == null || journalQueueConfiguration.JournalQueue == null)
			{
				return;
			}

			if (ShouldCreate(queueCreationType, journalQueueConfiguration.JournalQueue))
			{
				journalQueueConfiguration.JournalQueue.AttemptCreate();
			}
		}

		private static bool ShouldCreate(QueueCreationType queueCreationType, IQueue queue)
		{
			return (queueCreationType == QueueCreationType.All) ||
				   (queueCreationType == QueueCreationType.Local && queue.IsLocal);
		}

		public void RegisterQueueFactory(IQueueFactory queueFactory)
		{
			Guard.AgainstNull(queueFactory, "queueFactory");

			if (ContainsQueueFactory(queueFactory.Scheme))
			{
				throw new DuplicateQueueFactoryException(queueFactory);
			}

			_queueFactories.Add(queueFactory);
		}

		public bool ContainsQueueFactory(string scheme)
		{
			return _queueFactories.Find(
					   factory => factory.Scheme.Equals(scheme, StringComparison.InvariantCultureIgnoreCase)
					   ) != null;
		}

		public void Dispose()
		{
			_queueFactories.AttemptDispose();
			_queues.AttemptDispose();
		}

		public void Initialize(IServiceBus bus)
		{
			// todo: initialize the queues here... maybe have an internal registration to handle the duplicate business depending on whether the developer registered a factory
			throw new NotImplementedException();
		}
	}
}