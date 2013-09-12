using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class QueueManager : IQueueManager, IDisposable
	{
		private readonly IReflectionService reflectionService;
		private static readonly object padlock = new object();

		private readonly List<IQueue> queues = new List<IQueue>();
		private readonly List<IQueueFactory> queueFactories = new List<IQueueFactory>();
		private bool initialized;

		private readonly ILog log;

		public QueueManager(IReflectionService reflectionService)
		{
			Guard.AgainstNull(reflectionService, "reflectionService");

			this.reflectionService = reflectionService;

			log = Log.For(this);
		}

		public QueueManager()
			: this(new ReflectionService())
		{
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
			if (initialized)
			{
				return queueFactories;
			}

			lock (padlock)
			{
				if (!initialized)
				{
					var factoryTypes = new List<Type>();

					reflectionService.GetAssemblies(AppDomain.CurrentDomain.BaseDirectory).ForEach(
						assembly => factoryTypes.AddRange(reflectionService.GetTypes<IQueueFactory>(assembly)));

					foreach (var type in factoryTypes.Union(reflectionService.GetTypes<IQueueFactory>()))
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
							log.Warning(string.Format("Queue factory not instantiated: {0}", ex.Message));
						}
					}

					initialized = true;
				}
			}

			return queueFactories;
		}

		public IQueue GetQueue(string uri)
		{
			var queue =
				queues.Find(
					candidate => FindByString(candidate, uri));

			if (queue != null)
			{
				return queue;
			}

			lock (padlock)
			{
				queue =
					queues.Find(
						candidate => FindByString(candidate, uri));

				if (queue != null)
				{
					return queue;
				}

				var identifier = new Uri(uri);

				queue = GetQueueFactory(identifier).Create(identifier);

				queues.Add(queue);

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

		public void CreatePhysicalQueue(IQueue queue)
		{
			Guard.AgainstNull(queue, "queue");

			var operation = queue as ICreate;

			if (operation == null)
			{
				throw new InvalidOperationException(string.Format(ESBResources.NotImplementedOnQueue,
																  queue.GetType().FullName, "ICreate"));
			}

			operation.Create();
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
					CreatePhysicalQueue(serviceBusConfiguration.Worker.DistributorControlInboxWorkQueue);
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
				CreatePhysicalQueue(workQueueConfiguration.WorkQueue);
			}

			var errorQueueConfiguration = workQueueConfiguration as IErrorQueueConfiguration;

			if (errorQueueConfiguration != null)
			{
				if (ShouldCreate(queueCreationType, errorQueueConfiguration.ErrorQueue))
				{
					CreatePhysicalQueue(errorQueueConfiguration.ErrorQueue);
				}
			}

			var journalQueueConfiguration = workQueueConfiguration as IJournalQueueConfiguration;

			if (journalQueueConfiguration == null || journalQueueConfiguration.JournalQueue == null)
			{
				return;
			}

			if (ShouldCreate(queueCreationType, journalQueueConfiguration.JournalQueue))
			{
				CreatePhysicalQueue(journalQueueConfiguration.JournalQueue);
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

			if (
				queueFactories.Find(
					factory => factory.Scheme.Equals(queueFactory.Scheme, StringComparison.InvariantCultureIgnoreCase)) !=
				null)
			{
				throw new DuplicateQueueFactoryException(queueFactory);
			}

			queueFactories.Add(queueFactory);
		}

		public bool ContainsQueueFactory(string scheme)
		{
			return queueFactories.Find(
					   factory => factory.Scheme.Equals(scheme, StringComparison.InvariantCultureIgnoreCase)
					   ) != null;
		}

		public void Dispose()
		{
			queueFactories.AttemptDispose();
			queues.AttemptDispose();
		}
	}
}