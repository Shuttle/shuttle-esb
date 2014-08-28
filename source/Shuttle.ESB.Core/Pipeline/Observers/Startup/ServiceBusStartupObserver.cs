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
		private readonly ServiceBusConfiguration _configuration;

		private readonly ILog _log;

		public ServiceBusStartupObserver(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			_bus = bus;
			_configuration = (ServiceBusConfiguration) _bus.Configuration;
			_log = Log.For(this);
		}

		public void Execute(OnInitializeQueueFactories pipelineEvent)
		{
			_configuration.QueueManager.AttemptInitialization(_bus);

			foreach (var factory in _configuration.QueueManager.GetQueueFactories())
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
				_configuration.QueueManager.CreatePhysicalQueues(_configuration);
			}
		}

		public void Execute(OnInitializeMessageHandlerFactory pipelineEvent)
		{
			_configuration.MessageHandlerFactory.AttemptInitialization(_bus);
		}

		public void Execute(OnInitializePipelineFactory pipelineEvent)
		{
			_configuration.PipelineFactory.AttemptInitialization(_bus);
		}

		public void Execute(OnInitializeSubscriptionManager pipelineEvent)
		{
			if (!_configuration.HasSubscriptionManager)
			{
				_log.Information(ESBResources.NoSubscriptionManager);

				return;
			}

			_configuration.SubscriptionManager.AttemptInitialization(_bus);
		}

		public void Execute(OnInitializeIdempotenceService pipelineEvent)
		{
			if (!_configuration.HasIdempotenceService)
			{
				_log.Information(ESBResources.NoIdempotenceService);

				return;
			}

			_configuration.IdempotenceService.AttemptInitialization(_bus);
		}

		public void Execute(OnInitializeTransactionScopeFactory pipelineEvent)
		{
			_configuration.TransactionScopeFactory.AttemptInitialization(_bus);
		}

		public void Execute(OnStartInboxProcessing pipelineEvent)
		{
			if (!_configuration.HasInbox)
			{
				return;
			}

			var inbox = _configuration.Inbox;

			pipelineEvent.Pipeline.State.Add(
				"InboxThreadPool",
				new ProcessorThreadPool(
					"InboxProcessor",
					inbox.ThreadCount,
					new InboxProcessorFactory(_bus)).Start());
		}

		public void Execute(OnStartControlInboxProcessing pipelineEvent)
		{
			if (!_configuration.HasControlInbox)
			{
				return;
			}

			pipelineEvent.Pipeline.State.Add(
				"ControlInboxThreadPool",
				new ProcessorThreadPool(
					"ControlInboxProcessor",
					_configuration.ControlInbox.ThreadCount,
					new ControlInboxProcessorFactory(_bus)).Start());
		}

		public void Execute(OnStartOutboxProcessing pipelineEvent)
		{
			if (!_configuration.HasOutbox)
			{
				return;
			}

			pipelineEvent.Pipeline.State.Add(
				"OutboxThreadPool",
				new ProcessorThreadPool(
					"OutboxProcessor",
					_configuration.Outbox.ThreadCount,
					new OutboxProcessorFactory(_bus)).Start());
		}

		public void Execute(OnStartDeferredMessageProcessing pipelineEvent)
		{
			if (!_configuration.HasDeferredQueue)
			{
				return;
			}

			_configuration.DeferredMessageProcessor = new DeferredMessageProcessor(_bus);

			pipelineEvent.Pipeline.State.Add(
				"DeferredMessageThreadPool",
				new ProcessorThreadPool(
					"DeferredMessageProcessor",
					1,
					new DeferredMessageProcessorFactory(_bus)).Start());
		}

		public void Execute(OnStartWorker pipelineEvent)
		{
			if (!_configuration.IsWorker)
			{
				return;
			}

			_bus.Send(new WorkerStartedEvent
				{
					InboxWorkQueueUri = _configuration.Inbox.WorkQueue.Uri.ToString(),
					DateStarted = DateTime.Now
				},
			          c => c.WithRecipient(_configuration.Worker.DistributorControlInboxWorkQueue));
		}

		public void Execute(OnInitializeMessageRouteProvider pipelineEvent)
		{
			_configuration.MessageRouteProvider.AttemptInitialization(_bus);
		}

		public void Execute(OnInitializeForwardingRouteProvider pipelineEvent)
		{
			_configuration.ForwardingRouteProvider.AttemptInitialization(_bus);
		}

		public void Execute(OnRegisterSharedConfiguration pipelineEvent)
		{
			if (ServiceBusConfiguration.ServiceBusSection == null)
			{
				_configuration.RemoveMessagesNotHandled = false;

				return;
			}

			_configuration.RemoveMessagesNotHandled = ServiceBusConfiguration.ServiceBusSection.RemoveMessagesNotHandled;
			_configuration.CompressionAlgorithm = ServiceBusConfiguration.ServiceBusSection.CompressionAlgorithm;
			_configuration.EncryptionAlgorithm = ServiceBusConfiguration.ServiceBusSection.EncryptionAlgorithm;

			var transactionScopeElement = ServiceBusConfiguration.ServiceBusSection.TransactionScope;

			_configuration.TransactionScope = transactionScopeElement != null
				                                  ? new TransactionScopeConfiguration
					                                  {
						                                  Enabled = transactionScopeElement.Enabled,
						                                  IsolationLevel = transactionScopeElement.IsolationLevel,
						                                  TimeoutSeconds = transactionScopeElement.TimeoutSeconds
					                                  }
				                                  : new TransactionScopeConfiguration();
		}

		public void Execute(OnRegisterControlInboxQueueConfiguration pipelineEvent)
		{
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

			_configuration.ControlInbox =
				new ControlInboxQueueConfiguration
					{
						WorkQueue =
							_configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.ControlInbox.WorkQueueUri),
						ErrorQueue =
							_configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.ControlInbox.ErrorQueueUri),
						ThreadCount = ServiceBusConfiguration.ServiceBusSection.ControlInbox.ThreadCount,
						MaximumFailureCount = ServiceBusConfiguration.ServiceBusSection.ControlInbox.MaximumFailureCount,
						DurationToIgnoreOnFailure =
							ServiceBusConfiguration.ServiceBusSection.ControlInbox.DurationToIgnoreOnFailure ??
							defaultDurationToIgnoreOnFailure,
						DurationToSleepWhenIdle =
							ServiceBusConfiguration.ServiceBusSection.ControlInbox.DurationToSleepWhenIdle ?? defaultDurationToSleepWhenIdle
					};
		}

		public void Execute(OnRegisterInboxQueueConfiguration pipelineEvent)
		{
			if (ServiceBusConfiguration.ServiceBusSection == null
			    ||
			    ServiceBusConfiguration.ServiceBusSection.Inbox == null
			    ||
			    string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.Inbox.WorkQueueUri))
			{
				return;
			}

			_configuration.Inbox =
				new InboxQueueConfiguration
					{
						WorkQueue = _configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.Inbox.WorkQueueUri),
						ErrorQueue = _configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.Inbox.ErrorQueueUri),
						ThreadCount = ServiceBusConfiguration.ServiceBusSection.Inbox.ThreadCount,
						MaximumFailureCount = ServiceBusConfiguration.ServiceBusSection.Inbox.MaximumFailureCount,
						DurationToIgnoreOnFailure =
							ServiceBusConfiguration.ServiceBusSection.Inbox.DurationToIgnoreOnFailure ?? defaultDurationToIgnoreOnFailure,
						DurationToSleepWhenIdle =
							ServiceBusConfiguration.ServiceBusSection.Inbox.DurationToSleepWhenIdle ?? defaultDurationToSleepWhenIdle,
						Distribute = ServiceBusConfiguration.ServiceBusSection.Inbox.Distribute,
						DistributeSendCount = ServiceBusConfiguration.ServiceBusSection.Inbox.DistributeSendCount,
						DeferredQueue =
							string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.Inbox.DeferredQueueUri)
								? null
								: _configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.Inbox.DeferredQueueUri)
					};
		}

		public void Execute(OnRegisterOutboxQueueConfiguration pipelineEvent)
		{
			if (ServiceBusConfiguration.ServiceBusSection == null
			    ||
			    ServiceBusConfiguration.ServiceBusSection.Outbox == null
			    ||
			    string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.Outbox.WorkQueueUri))
			{
				return;
			}

			_configuration.Outbox =
				new OutboxQueueConfiguration
					{
						WorkQueue = _configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.Outbox.WorkQueueUri),
						ErrorQueue = _configuration.QueueManager.GetQueue(ServiceBusConfiguration.ServiceBusSection.Outbox.ErrorQueueUri),
						MaximumFailureCount = ServiceBusConfiguration.ServiceBusSection.Outbox.MaximumFailureCount,
						DurationToIgnoreOnFailure =
							ServiceBusConfiguration.ServiceBusSection.Outbox.DurationToIgnoreOnFailure ?? defaultDurationToIgnoreOnFailure,
						DurationToSleepWhenIdle =
							ServiceBusConfiguration.ServiceBusSection.Outbox.DurationToSleepWhenIdle ?? defaultDurationToSleepWhenIdle,
						ThreadCount = ServiceBusConfiguration.ServiceBusSection.Inbox.ThreadCount
					};
		}

		public void Execute(OnRegisterWorkerConfiguration pipelineEvent)
		{
			if (ServiceBusConfiguration.ServiceBusSection == null
			    ||
			    ServiceBusConfiguration.ServiceBusSection.Worker == null
			    ||
			    string.IsNullOrEmpty(ServiceBusConfiguration.ServiceBusSection.Worker.DistributorControlWorkQueueUri))
			{
				return;
			}

			_configuration.Worker =
				new WorkerConfiguration(_configuration.QueueManager.CreateQueue(
					ServiceBusConfiguration.ServiceBusSection.Worker.DistributorControlWorkQueueUri),
				                        ServiceBusConfiguration.ServiceBusSection.Worker.ThreadAvailableNotificationIntervalSeconds);
		}
	}
}