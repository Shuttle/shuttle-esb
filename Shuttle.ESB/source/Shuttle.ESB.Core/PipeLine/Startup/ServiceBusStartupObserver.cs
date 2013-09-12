using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class ServiceBusStartupObserver :
		IPipelineObserver<OnInitializeQueueFactories>,
		IPipelineObserver<OnInitializeMessageSerializer>,
		IPipelineObserver<OnCreateQueues>,
		IPipelineObserver<OnInitializeMessageHandlerFactory>,
		IPipelineObserver<OnInitializeMessageRouteProvider>,
		IPipelineObserver<OnInitializeModules>,
		IPipelineObserver<OnInitializePipelineFactory>,
		IPipelineObserver<OnInitializeSubscriptionManager>,
		IPipelineObserver<OnStartInboxProcessing>,
		IPipelineObserver<OnStartControlInboxProcessing>,
		IPipelineObserver<OnStartOutboxProcessing>,
		IPipelineObserver<OnStartWorker>,
		IPipelineObserver<OnRecoverInboxJournal>,
		IPipelineObserver<OnRecoverControlInboxJournal>
	{
		private readonly IServiceBus bus;

		public ServiceBusStartupObserver(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			this.bus = bus;
		}

		public void Execute(OnInitializeQueueFactories pipelineEvent)
		{
			var memoryQueueFactory = new MemoryQueueFactory();

			if (!bus.Configuration.Queues.ContainsQueueFactory(memoryQueueFactory.Scheme))
			{
				bus.Configuration.Queues.RegisterQueueFactory(memoryQueueFactory);
			}

			foreach (var factory in bus.Configuration.Queues.GetQueueFactories())
			{
				var initialization = factory as IRequireInitialization;

				if (initialization != null)
				{
					initialization.Initialize(bus);
				}
			}
		}

		public void Execute(OnInitializeMessageSerializer pipelineEvent)
		{
			var serializer = bus.Configuration.MessageSerializer as IKnownTypes;

			if (serializer != null)
			{
				serializer.AddKnownTypes(bus.Configuration.KnownMessageTypes);
			}
		}

		public void Execute(OnCreateQueues pipelineEvent)
		{
			if (ServiceBusConfiguration.ServiceBusSection != null)
			{
				bus.Configuration.Queues.CreatePhysicalQueues(ServiceBusConfiguration.ServiceBusSection.QueueCreationType);
			}
		}

		public void Execute(OnInitializeMessageHandlerFactory pipelineEvent)
		{
			bus.Configuration.MessageHandlerFactory.Initialize(bus);
		}

		public void Execute(OnInitializeModules pipelineEvent)
		{
			foreach (var module in bus.Configuration.Modules)
			{
				module.Initialize(bus);
			}
		}

		public void Execute(OnInitializePipelineFactory pipelineEvent)
		{
		}

		public void Execute(OnInitializeSubscriptionManager pipelineEvent)
		{
			if (!bus.Configuration.HasSubscriptionManager
			    ||
			    ServiceBusConfiguration.ServiceBusSection == null
			    ||
			    !ServiceBusConfiguration.ServiceBusSection.Subscription.RegisterSubscriptions)
			{
				return;
			}

			var eventType = typeof (IEventMessage);

			foreach (var type in bus.Configuration.MessageHandlerFactory.MessageTypesHandled)
			{
				if (type.IsAssignableTo(eventType))
				{
					bus.Configuration.SubscriptionManager.Subscribe(new[] {type.FullName});
				}
			}

			var initialization = bus.Configuration.SubscriptionManager as IRequireInitialization;

			if (initialization != null)
			{
				initialization.Initialize(bus);
			}
		}

		public void Execute(OnStartInboxProcessing pipelineEvent)
		{
			if (!bus.Configuration.HasInbox)
			{
				return;
			}

			var inbox = bus.Configuration.Inbox;

			if (inbox.WorkQueueStartupAction == QueueStartupAction.Purge)
			{
				var queue = inbox.WorkQueue as IPurge;

				if (queue != null)
				{
					Log.Information(string.Format(Resources.PurgingInboxWorkQueue, inbox.WorkQueue.Uri));

					queue.Purge();

					Log.Information(string.Format(Resources.PurgingInboxWorkQueueComplete, inbox.WorkQueue.Uri));
				}
				else
				{
					Log.Warning(string.Format(Resources.CannotPurgeQueue, inbox.WorkQueue.Uri));
				}
			}

			pipelineEvent.PipelineState.Add(
				"InboxThreadPool",
				!inbox.Distribute
					? new ProcessorThreadPool(
					  	"InboxProcessor",
					  	inbox.ThreadCount,
					  	new InboxProcessorFactory(bus)).Start()
					: new ProcessorThreadPool(
					  	"DistributorProcessor",
					  	1,
					  	new DistributorProcessorFactory(bus)).Start());
		}

		public void Execute(OnStartControlInboxProcessing pipelineEvent)
		{
			if (!bus.Configuration.HasControlInbox)
			{
				return;
			}

			pipelineEvent.PipelineState.Add(
				"ControlInboxThreadPool",
				new ProcessorThreadPool(
					"ControlProcessor",
					1,
					new ControlInboxProcessorFactory(bus)).Start());
		}

		public void Execute(OnStartOutboxProcessing pipelineEvent)
		{
			if (!bus.Configuration.HasOutbox)
			{
				return;
			}

			pipelineEvent.PipelineState.Add(
				"OutboxThreadPool",
				new ProcessorThreadPool(
					"OutboxProcessor",
					1,
					new OutboxProcessorFactory(bus)).Start());
		}

		public void Execute(OnStartWorker pipelineEvent)
		{
			if (!bus.Configuration.IsWorker)
			{
				return;
			}

			bus.Send(new WorkerStartedEvent
			         	{
			         		InboxWorkQueueUri = bus.Configuration.Inbox.WorkQueue.Uri.ToString(),
			         		DateStarted = DateTime.Now
			         	},
			         bus.Configuration.Worker.DistributorControlInboxWorkQueue);
		}

		public void Execute(OnInitializeMessageRouteProvider pipelineEvent)
		{
			bus.Configuration.MessageRouteProvider.AttemptInitialization(bus);
		}

		public void Execute(OnRecoverInboxJournal pipelineEvent)
		{
			if (bus.Configuration.HasInbox)
			{
				RecoverJournal(bus.Configuration.Inbox.WorkQueue, bus.Configuration.Inbox.JournalQueue);
			}
		}

		public void Execute(OnRecoverControlInboxJournal pipelineEvent)
		{
			if (bus.Configuration.HasControlInbox)
			{
				RecoverJournal(bus.Configuration.ControlInbox.WorkQueue, bus.Configuration.ControlInbox.JournalQueue);
			}
		}

		private void RecoverJournal(IQueue workerQueue, IQueue journalQueue)
		{
			var recoverCount = 0;
			var stream = journalQueue.Dequeue();

			while (stream != null)
			{
				var transportMessage = (TransportMessage)bus.Configuration.MessageSerializer.Deserialize(typeof(TransportMessage), stream);

				workerQueue.Enqueue(transportMessage.MessageId, stream);

				stream = journalQueue.Dequeue();

				recoverCount++;
			}

			Log.Information(string.Format(Resources.JournalRecoverCount, recoverCount, workerQueue.Uri));
		}
	}
}