using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class ServiceBusStartupObserver :
		IPipelineObserver<OnRegisterSharedConfiguration>,
		IPipelineObserver<OnRegisterControlInboxQueueConfiguration>,
		IPipelineObserver<OnRegisterInboxQueueConfiguration>,
		IPipelineObserver<OnRegisterOutboxQueueConfiguration>,
		IPipelineObserver<OnRegisterWorkerConfiguration>,
		IPipelineObserver<OnInitializeQueueFactories>,
		IPipelineObserver<OnCreateQueues>,
		IPipelineObserver<OnInitializeMessageHandlerFactory>,
		IPipelineObserver<OnInitializeMessageRouteProvider>,
		IPipelineObserver<OnInitializeForwardingRouteProvider>,
		IPipelineObserver<OnInitializePipelineFactory>,
		IPipelineObserver<OnInitializeSubscriptionManager>,
		IPipelineObserver<OnInitializeQueueManager>,
		IPipelineObserver<OnInitializeIdempotenceService>,
		IPipelineObserver<OnInitializeTransactionScopeFactory>,
		IPipelineObserver<OnStartInboxProcessing>,
		IPipelineObserver<OnStartControlInboxProcessing>,
		IPipelineObserver<OnStartOutboxProcessing>,
		IPipelineObserver<OnStartDeferredMessageProcessing>,
		IPipelineObserver<OnStartWorker>
	{
		private readonly TimeSpan[] defaultDurationToIgnoreOnFailure =
			new[]
				{
					TimeSpan.FromMinutes(5),
					TimeSpan.FromMinutes(10),
					TimeSpan.FromMinutes(15),
					TimeSpan.FromMinutes(30),
					TimeSpan.FromMinutes(60)
				};

		private readonly TimeSpan[] defaultDurationToSleepWhenIdle =
			(TimeSpan[])
			new StringDurationArrayConverter()
				.ConvertFrom("250ms*4,500ms*2,1s");

		private readonly IServiceBus _bus;

		private readonly ILog _log;

		public ServiceBusStartupObserver(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			_bus = bus;
			_log = Log.For(this);
		}

		public void Execute(OnInitializeQueueFactories pipelineEvent)
		{
			_bus.Configuration.QueueManager.AttemptInitialization(_bus);

			foreach (var factory in _bus.Configuration.QueueManager.GetQueueFactories())
			{
				factory.AttemptInitialization(_bus);
			}
		}

		public void Execute(OnCreateQueues pipelineEvent)
		{
			if (ServiceBusConfiguration.ServiceBusSection != null
				&&
				ServiceBusConfiguration.ServiceBusSection.CreateQueues)
			{
				_bus.Configuration.QueueManager.CreatePhysicalQueues(_bus.Configuration);
			}
		}

		public void Execute(OnInitializeMessageHandlerFactory pipelineEvent)
		{
			_bus.Configuration.MessageHandlerFactory.AttemptInitialization(_bus);
		}

		public void Execute(OnInitializePipelineFactory pipelineEvent)
		{
			_bus.Configuration.PipelineFactory.AttemptInitialization(_bus);
		}

		public void Execute(OnInitializeSubscriptionManager pipelineEvent)
		{
			if (!_bus.Configuration.HasSubscriptionManager)
			{
				_log.Information(ESBResources.NoSubscriptionManager);

				return;
			}

			_bus.Configuration.SubscriptionManager.AttemptInitialization(_bus);
		}

		public void Execute(OnInitializeIdempotenceService pipelineEvent)
		{
			if (!_bus.Configuration.HasIdempotenceService)
			{
				_log.Information(ESBResources.NoIdempotenceService);

				return;
			}

			_bus.Configuration.IdempotenceService.AttemptInitialization(_bus);
		}

		public void Execute(OnInitializeTransactionScopeFactory pipelineEvent)
		{
			_bus.Configuration.TransactionScopeFactory.AttemptInitialization(_bus);
		}

		public void Execute(OnStartInboxProcessing pipelineEvent)
		{
			if (!_bus.Configuration.HasInbox)
			{
				return;
			}

			var inbox = _bus.Configuration.Inbox;

			if (inbox.WorkQueueStartupAction == QueueStartupAction.Purge)
			{
				var queue = inbox.WorkQueue as IPurge;

				if (queue != null)
				{
					_log.Information(string.Format(ESBResources.PurgingInboxWorkQueue, inbox.WorkQueue.Uri));

					queue.Purge();

					_log.Information(string.Format(ESBResources.PurgingInboxWorkQueueComplete, inbox.WorkQueue.Uri));
				}
				else
				{
					_log.Warning(string.Format(ESBResources.CannotPurgeQueue, inbox.WorkQueue.Uri));
				}
			}

			pipelineEvent.Pipeline.State.Add(
				"InboxThreadPool",
					 new ProcessorThreadPool(
						"InboxProcessor",
						inbox.ThreadCount,
						new InboxProcessorFactory(_bus)).Start());
		}

		public void Execute(OnStartControlInboxProcessing pipelineEvent)
		{
			if (!_bus.Configuration.HasControlInbox)
			{
				return;
			}

			pipelineEvent.Pipeline.State.Add(
				"ControlInboxThreadPool",
				new ProcessorThreadPool(
					"ControlInboxProcessor",
					_bus.Configuration.ControlInbox.ThreadCount,
					new ControlInboxProcessorFactory(_bus)).Start());
		}

		public void Execute(OnStartOutboxProcessing pipelineEvent)
		{
			if (!_bus.Configuration.HasOutbox)
			{
				return;
			}

			pipelineEvent.Pipeline.State.Add(
				"OutboxThreadPool",
				new ProcessorThreadPool(
					"OutboxProcessor",
					_bus.Configuration.Outbox.ThreadCount,
					new OutboxProcessorFactory(_bus)).Start());
		}

		public void Execute(OnStartDeferredMessageProcessing pipelineEvent)
		{
			if (!_bus.Configuration.HasDeferredQueue || _bus.Configuration.IsWorker)
			{
				return;
			}

			pipelineEvent.Pipeline.State.Add(
				"DeferredMessageThreadPool",
				new ProcessorThreadPool(
					"DeferredMessageProcessor",
					1,
					new DeferredMessageProcessorFactory(_bus)).Start());
		}

		public void Execute(OnStartWorker pipelineEvent)
		{
			if (!_bus.Configuration.IsWorker)
			{
				return;
			}

			_bus.Send(new WorkerStartedEvent
						{
							InboxWorkQueueUri = _bus.Configuration.Inbox.WorkQueue.Uri.ToString(),
							DateStarted = DateTime.Now
						},
					 _bus.Configuration.Worker.DistributorControlInboxWorkQueue);
		}

		public void Execute(OnInitializeMessageRouteProvider pipelineEvent)
		{
			_bus.Configuration.MessageRouteProvider.AttemptInitialization(_bus);
		}

		public void Execute(OnInitializeForwardingRouteProvider pipelineEvent)
		{
			_bus.Configuration.ForwardingRouteProvider.AttemptInitialization(_bus);
		}

		public void Execute(OnInitializeQueueManager pipelineEvent)
		{
			_bus.Configuration.QueueManager.AttemptInitialization(_bus);
		}

		public void Execute(OnRegisterSharedConfiguration pipelineEvent)
		{
			var configuration = (ServiceBusConfiguration)pipelineEvent.GetServiceBus().Configuration;

			if (ServiceBusConfiguration.ServiceBusSection == null)
			{
				configuration.RemoveMessagesNotHandled = false;

				return;
			}

			configuration.RemoveMessagesNotHandled = ServiceBusConfiguration.ServiceBusSection.RemoveMessagesNotHandled;
			configuration.CompressionAlgorithm = ServiceBusConfiguration.ServiceBusSection.CompressionAlgorithm;
			configuration.EncryptionAlgorithm = ServiceBusConfiguration.ServiceBusSection.EncryptionAlgorithm;
			configuration.TransactionScope = new TransactionScopeConfiguration
			{
				Enabled = ServiceBusConfiguration.ServiceBusSection.TransactionScope.Enabled,
				IsolationLevel = ServiceBusConfiguration.ServiceBusSection.TransactionScope.IsolationLevel,
				TimeoutSeconds = ServiceBusConfiguration.ServiceBusSection.TransactionScope.TimeoutSeconds
			};
		}

		public void Execute(OnRegisterControlInboxQueueConfiguration pipelineEvent)
		{
			var configuration = (ServiceBusConfiguration)pipelineEvent.GetServiceBus().Configuration;

			if (ServiceBusConfiguration.ServiceBusSection == null
			||
			ServiceBusConfiguration.ServiceBusSection.ControlInbox == null
			||
			string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.ControlInbox.WorkQueueUri)
			||
			string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.ControlInbox.ErrorQueueUri))
			{
				return;
			}

			configuration.ControlInbox =
				new ControlInboxQueueConfiguration
				{
					WorkQueue = configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.ControlInbox.WorkQueueUri),
					ErrorQueue = configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.ControlInbox.ErrorQueueUri),
					ThreadCount = ServiceBusConfiguration.ServiceBusSection.ControlInbox.ThreadCount,
					MaximumFailureCount = ServiceBusConfiguration.ServiceBusSection.ControlInbox.MaximumFailureCount,
					DurationToIgnoreOnFailure = ServiceBusConfiguration.ServiceBusSection.ControlInbox.DurationToIgnoreOnFailure ?? defaultDurationToIgnoreOnFailure,
					DurationToSleepWhenIdle = ServiceBusConfiguration.ServiceBusSection.ControlInbox.DurationToSleepWhenIdle ?? defaultDurationToSleepWhenIdle
				};
		}

		public void Execute(OnRegisterInboxQueueConfiguration pipelineEvent)
		{
			var configuration = (ServiceBusConfiguration)pipelineEvent.GetServiceBus().Configuration;

			if (ServiceBusConfiguration.ServiceBusSection == null
			||
			ServiceBusConfiguration.ServiceBusSection.Inbox == null
			||
			string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.Inbox.WorkQueueUri))
			{
				return;
			}

			configuration.Inbox =
				new InboxQueueConfiguration
				{
					WorkQueue = configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.Inbox.WorkQueueUri),
					ErrorQueue = configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.Inbox.ErrorQueueUri),
					WorkQueueStartupAction = ServiceBusConfiguration.ServiceBusSection.Inbox.WorkQueueStartupAction,
					ThreadCount = ServiceBusConfiguration.ServiceBusSection.Inbox.ThreadCount,
					MaximumFailureCount = ServiceBusConfiguration.ServiceBusSection.Inbox.MaximumFailureCount,
					DurationToIgnoreOnFailure = ServiceBusConfiguration.ServiceBusSection.Inbox.DurationToIgnoreOnFailure ?? defaultDurationToIgnoreOnFailure,
					DurationToSleepWhenIdle = ServiceBusConfiguration.ServiceBusSection.Inbox.DurationToSleepWhenIdle ?? defaultDurationToSleepWhenIdle,
					Distribute = ServiceBusConfiguration.ServiceBusSection.Inbox.Distribute,
					DeferredQueue =
						string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.Inbox.DeferredQueueUri)
							? null
							: configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.Inbox.DeferredQueueUri)
				};
		}

		public void Execute(OnRegisterOutboxQueueConfiguration pipelineEvent)
		{
			var configuration = (ServiceBusConfiguration)pipelineEvent.GetServiceBus().Configuration;

			if (ServiceBusConfiguration.ServiceBusSection == null
		||
		ServiceBusConfiguration.ServiceBusSection.Outbox == null
		||
		string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.Outbox.WorkQueueUri))
			{
				return;
			}

			configuration.Outbox =
				new OutboxQueueConfiguration
				{
					WorkQueue = configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.Outbox.WorkQueueUri),
					ErrorQueue = configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.Outbox.ErrorQueueUri),
					MaximumFailureCount = ServiceBusConfiguration.ServiceBusSection.Outbox.MaximumFailureCount,
					DurationToIgnoreOnFailure = ServiceBusConfiguration.ServiceBusSection.Outbox.DurationToIgnoreOnFailure ?? defaultDurationToIgnoreOnFailure,
					DurationToSleepWhenIdle = ServiceBusConfiguration.ServiceBusSection.Outbox.DurationToSleepWhenIdle ?? defaultDurationToSleepWhenIdle,
					ThreadCount = ServiceBusConfiguration.ServiceBusSection.Inbox.ThreadCount
				};
		}

		public void Execute(OnRegisterWorkerConfiguration pipelineEvent)
		{
			var configuration = (ServiceBusConfiguration)pipelineEvent.GetServiceBus().Configuration;

			if (ServiceBusConfiguration.ServiceBusSection == null
			||
			ServiceBusConfiguration.ServiceBusSection.Worker == null
			||
			string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.Worker.DistributorControlWorkQueueUri))
			{
				return;
			}

			configuration.Worker =
				new WorkerConfiguration(configuration.QueueManager.CreateQueue(
						ServiceBusConfiguration.ServiceBusSection.Worker.DistributorControlWorkQueueUri),
						ServiceBusConfiguration.ServiceBusSection.Worker.ThreadAvailableNotificationIntervalSeconds);
		}
	}
}