using System;
using System.Transactions;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class DefaultServiceBusTransactionScopeFactory : IServiceBusTransactionScopeFactory, IRequireInitialization
    {
	    private bool _enabled ;
        private IsolationLevel _isolationLevel;
        private TimeSpan _timeout;
		private readonly IServiceBusTransactionScope _nullServiceBusTransactionScope = new NullServiceBusTransactionScope();

	    public IServiceBusTransactionScope Create(PipelineEvent pipelineEvent)
        {
			return _enabled ? new ServiceBusTransactionScope(_isolationLevel, _timeout) : _nullServiceBusTransactionScope;
		}

        public void Initialize(IServiceBus bus)
        {
	        _enabled = bus.Configuration.TransactionScope.Enabled;
			_isolationLevel = bus.Configuration.TransactionScope.IsolationLevel;
			_timeout = TimeSpan.FromSeconds(bus.Configuration.TransactionScope.TimeoutSeconds);
        }
    }
}