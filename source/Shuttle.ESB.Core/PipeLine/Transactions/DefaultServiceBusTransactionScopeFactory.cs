using System;
using System.Transactions;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DefaultServiceBusTransactionScopeFactory : IServiceBusTransactionScopeFactory, IRequireInitialization
    {
        private IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted;
        private TimeSpan timeout = TimeSpan.FromSeconds(30);

    	public ServiceBusTransactionScope Create()
    	{
			return new ServiceBusTransactionScope(isolationLevel, timeout);
    	}

    	public ServiceBusTransactionScope Create(object message)
        {
            return new ServiceBusTransactionScope(isolationLevel, timeout);
        }

        public ServiceBusTransactionScope Create(PipelineEvent pipelineEvent)
        {
            return new ServiceBusTransactionScope(isolationLevel, timeout);
        }

        public void Initialize(IServiceBus bus)
        {
            isolationLevel = ServiceBusConfiguration.ServiceBusSection.TransactionScope.IsolationLevel;
            timeout = ServiceBusConfiguration.ServiceBusSection.TransactionScope.TimeoutSeconds;
        }
    }
}