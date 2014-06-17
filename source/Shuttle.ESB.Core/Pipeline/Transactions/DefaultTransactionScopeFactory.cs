using System;
using System.Transactions;

namespace Shuttle.ESB.Core
{
	public class DefaultTransactionScopeFactory : ITransactionScopeFactory, IRequireInitialization
	{
		private bool _enabled;
		private IsolationLevel _isolationLevel;
		private TimeSpan _timeout;
		private readonly ITransactionScope _nullServiceBusTransactionScope = new NullTransactionScope();

		public ITransactionScope Create(PipelineEvent pipelineEvent)
		{
			return _enabled ? new DefaultTransactionScope(_isolationLevel, _timeout) : _nullServiceBusTransactionScope;
		}

		public void Initialize(IServiceBus bus)
		{
			_enabled = bus.Configuration.TransactionScope.Enabled;
			_isolationLevel = bus.Configuration.TransactionScope.IsolationLevel;
			_timeout = TimeSpan.FromSeconds(bus.Configuration.TransactionScope.TimeoutSeconds);
		}
	}
}