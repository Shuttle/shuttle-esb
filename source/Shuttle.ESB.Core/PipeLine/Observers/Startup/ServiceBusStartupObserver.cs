using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class ServiceBusStartupObserver :
        IPipelineObserver<OnInitializeQueueFactories>,
        IPipelineObserver<OnCreateQueues>,
        IPipelineObserver<OnInitializeMessageHandlerFactory>,
        IPipelineObserver<OnInitializeMessageRouteProvider>,
        IPipelineObserver<OnInitializeForwardingRouteProvider>,
        IPipelineObserver<OnInitializePipelineFactory>,
        IPipelineObserver<OnInitializeSubscriptionManager>,
        IPipelineObserver<OnInitializeQueueManager>,
        IPipelineObserver<OnInitializeIdempotenceTracker>,
        IPipelineObserver<OnInitializeTransactionScopeFactory>,
        IPipelineObserver<OnStartInboxProcessing>,
        IPipelineObserver<OnStartControlInboxProcessing>,
        IPipelineObserver<OnStartOutboxProcessing>,
		IPipelineObserver<OnStartDeferredMessageProcessing>,
        IPipelineObserver<OnStartWorker>,
        IPipelineObserver<OnRecoverInboxJournal>,
        IPipelineObserver<OnRecoverControlInboxJournal>
    {
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
            var memoryQueueFactory = new MemoryQueueFactory();

            if (!_bus.Configuration.QueueManager.ContainsQueueFactory(memoryQueueFactory.Scheme))
            {
                _bus.Configuration.QueueManager.RegisterQueueFactory(memoryQueueFactory);
            }

            foreach (var factory in _bus.Configuration.QueueManager.GetQueueFactories())
            {
                factory.AttemptInitialization(_bus);
            }
        }

        public void Execute(OnCreateQueues pipelineEvent)
        {
            if (ServiceBusConfiguration.ServiceBusSection != null)
            {
                _bus.Configuration.QueueManager.CreatePhysicalQueues(_bus.Configuration,
                                                                    ServiceBusConfiguration.ServiceBusSection.QueueCreationType);
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

        public void Execute(OnInitializeIdempotenceTracker pipelineEvent)
        {
            if (!_bus.Configuration.HasIdempotenceTracker)
            {
                _log.Information(ESBResources.NoIdempotenceTracker);

                return;
            }

            _bus.Configuration.IdempotenceTracker.AttemptInitialization(_bus);
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
            if (!_bus.Configuration.HasDeferredMessageQueue || _bus.Configuration.IsWorker)
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

        public void Execute(OnRecoverInboxJournal pipelineEvent)
        {
            if (_bus.Configuration.HasInbox && _bus.Configuration.Inbox.HasJournalQueue)
            {
                RecoverJournal(_bus.Configuration.Inbox.WorkQueue, _bus.Configuration.Inbox.JournalQueue);
            }
        }

        public void Execute(OnRecoverControlInboxJournal pipelineEvent)
        {
            if (_bus.Configuration.HasControlInbox && _bus.Configuration.ControlInbox.HasJournalQueue)
            {
                RecoverJournal(_bus.Configuration.ControlInbox.WorkQueue, _bus.Configuration.ControlInbox.JournalQueue);
            }
        }

        private void RecoverJournal(IQueue workerQueue, IQueue journalQueue)
        {
            var recoverCount = 0;
            var stream = journalQueue.Dequeue();

            while (stream != null)
            {
                var transportMessage =
                    (TransportMessage)_bus.Configuration.Serializer.Deserialize(typeof(TransportMessage), stream);

                workerQueue.Enqueue(transportMessage.MessageId, stream);

                stream = journalQueue.Dequeue();

                recoverCount++;
            }

            _log.Information(string.Format(ESBResources.JournalRecoverCount, recoverCount, workerQueue.Uri));
        }

        public void Execute(OnInitializeForwardingRouteProvider pipelineEvent1)
        {
            _bus.Configuration.ForwardingRouteProvider.AttemptInitialization(_bus);
        }

	    public void Execute(OnInitializeQueueManager pipelineEvent1)
	    {
			_bus.Configuration.QueueManager.AttemptInitialization(_bus);
	    }
    }
}