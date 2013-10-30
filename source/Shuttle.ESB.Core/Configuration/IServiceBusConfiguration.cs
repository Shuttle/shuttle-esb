using System;
using System.Collections.Generic;

namespace Shuttle.ESB.Core
{
    public interface IServiceBusConfiguration
    {
        bool HasServiceBusSection { get; }

        bool HasInbox { get; }
        bool HasControlInbox { get; }
        bool HasOutbox { get; }
        bool HasIdempotenceTracker { get; }
        bool HasSubscriptionManager { get; }
        bool IsWorker { get; }
        bool RemoveMessagesNotHandled { get; }

        IServiceBusPolicy Policy { get; }
        ISerializer Serializer { get; }
        IMessageRouteProvider MessageRouteProvider { get; }
        IMessageRouteProvider ForwardingRouteProvider { get; }

        IControlInboxQueueConfiguration ControlInbox { get; }
        IInboxQueueConfiguration Inbox { get; }
        IOutboxQueueConfiguration Outbox { get; }

        IMessageHandlerFactory MessageHandlerFactory { get; }
        IThreadActivityFactory ThreadActivityFactory { get; }

        IIdempotenceTracker IdempotenceTracker { get; }
        ISubscriptionManager SubscriptionManager { get; }
        IWorkerConfiguration Worker { get; }

        IWorkerAvailabilityManager WorkerAvailabilityManager { get; }
        IPipelineFactory PipelineFactory { get; }

        ModuleCollection Modules { get; }
        string OutgoingEncryptionAlgorithm { get; }
        string OutgoingCompressionAlgorithm { get; }
        IServiceBusTransactionScopeFactory TransactionScopeFactory { get; }
        string EncryptionAlgorithm { get; }
        string CompressionAlgorithm { get; }

        IEncryptionAlgorithm FindEncryptionAlgorithm(string name);
        void AddEncryptionAlgorithm(IEncryptionAlgorithm algorithm);

        ICompressionAlgorithm FindCompressionAlgorithm(string name);
        void AddCompressionAlgorithm(ICompressionAlgorithm algorithm);

        IServiceBus StartServiceBus();
    }
}