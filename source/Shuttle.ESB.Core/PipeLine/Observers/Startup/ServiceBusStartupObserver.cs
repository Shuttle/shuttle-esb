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
        IPipelineObserver<OnInitializeIdempotenceTracker>,
        IPipelineObserver<OnInitializeTransactionScopeFactory>,
        IPipelineObserver<OnStartInboxProcessing>,
        IPipelineObserver<OnStartControlInboxProcessing>,
        IPipelineObserver<OnStartOutboxProcessing>,
        IPipelineObserver<OnStartWorker>,
        IPipelineObserver<OnRecoverInboxJournal>,
        IPipelineObserver<OnRecoverControlInboxJournal>
    {
        private readonly IServiceBus bus;

        private readonly ILog log;

        public ServiceBusStartupObserver(IServiceBus bus)
        {
            Guard.AgainstNull(bus, "bus");

            this.bus = bus;

            log = Log.For(this);
        }

        public void Execute(OnInitializeQueueFactories pipelineEvent)
        {
            var memoryQueueFactory = new MemoryQueueFactory();

            if (!bus.Configuration.QueueManager.ContainsQueueFactory(memoryQueueFactory.Scheme))
            {
                bus.Configuration.QueueManager.RegisterQueueFactory(memoryQueueFactory);
            }

            foreach (var factory in bus.Configuration.QueueManager.GetQueueFactories())
            {
                factory.AttemptInitialization(bus);
            }
        }

        public void Execute(OnCreateQueues pipelineEvent)
        {
            if (ServiceBusConfiguration.ServiceBusSection != null)
            {
                bus.Configuration.QueueManager.CreatePhysicalQueues(bus.Configuration,
                                                                    ServiceBusConfiguration.ServiceBusSection.QueueCreationType);
            }
        }

        public void Execute(OnInitializeMessageHandlerFactory pipelineEvent)
        {
			bus.Configuration.MessageHandlerFactory.AttemptInitialization(bus);
        }

        public void Execute(OnInitializePipelineFactory pipelineEvent)
        {
            bus.Configuration.PipelineFactory.AttemptInitialization(bus);
        }

        public void Execute(OnInitializeSubscriptionManager pipelineEvent)
        {
            if (!bus.Configuration.HasSubscriptionManager)
            {
                log.Information(ESBResources.NoSubscriptionManager);

                return;
            }

            bus.Configuration.SubscriptionManager.AttemptInitialization(bus);
        }

        public void Execute(OnInitializeIdempotenceTracker pipelineEvent)
        {
            if (!bus.Configuration.HasIdempotenceTracker)
            {
                log.Information(ESBResources.NoIdempotenceTracker);

                return;
            }

            bus.Configuration.IdempotenceTracker.AttemptInitialization(bus);
        }

        public void Execute(OnInitializeTransactionScopeFactory pipelineEvent)
        {
            bus.Configuration.TransactionScopeFactory.AttemptInitialization(bus);
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
                    log.Information(string.Format(ESBResources.PurgingInboxWorkQueue, inbox.WorkQueue.Uri));

                    queue.Purge();

                    log.Information(string.Format(ESBResources.PurgingInboxWorkQueueComplete, inbox.WorkQueue.Uri));
                }
                else
                {
                    log.Warning(string.Format(ESBResources.CannotPurgeQueue, inbox.WorkQueue.Uri));
                }
            }

            pipelineEvent.Pipeline.State.Add(
                "InboxThreadPool",
                     new ProcessorThreadPool(
                        "InboxProcessor",
                        inbox.ThreadCount,
                        new InboxProcessorFactory(bus)).Start());
        }

        public void Execute(OnStartControlInboxProcessing pipelineEvent)
        {
            if (!bus.Configuration.HasControlInbox)
            {
                return;
            }

            pipelineEvent.Pipeline.State.Add(
                "ControlInboxThreadPool",
                new ProcessorThreadPool(
                    "ControlInboxProcessor",
                    bus.Configuration.ControlInbox.ThreadCount,
                    new ControlInboxProcessorFactory(bus)).Start());
        }

        public void Execute(OnStartOutboxProcessing pipelineEvent)
        {
            if (!bus.Configuration.HasOutbox)
            {
                return;
            }

            pipelineEvent.Pipeline.State.Add(
                "OutboxThreadPool",
                new ProcessorThreadPool(
                    "OutboxProcessor",
                    bus.Configuration.Outbox.ThreadCount,
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
            if (bus.Configuration.HasInbox && bus.Configuration.Inbox.HasJournalQueue)
            {
                RecoverJournal(bus.Configuration.Inbox.WorkQueue, bus.Configuration.Inbox.JournalQueue);
            }
        }

        public void Execute(OnRecoverControlInboxJournal pipelineEvent)
        {
            if (bus.Configuration.HasControlInbox && bus.Configuration.ControlInbox.HasJournalQueue)
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
                var transportMessage =
                    (TransportMessage)bus.Configuration.Serializer.Deserialize(typeof(TransportMessage), stream);

                workerQueue.Enqueue(transportMessage.MessageId, stream);

                stream = journalQueue.Dequeue();

                recoverCount++;
            }

            log.Information(string.Format(ESBResources.JournalRecoverCount, recoverCount, workerQueue.Uri));
        }

        public void Execute(OnInitializeForwardingRouteProvider pipelineEvent1)
        {
            bus.Configuration.ForwardingRouteProvider.AttemptInitialization(bus);
        }
    }
}