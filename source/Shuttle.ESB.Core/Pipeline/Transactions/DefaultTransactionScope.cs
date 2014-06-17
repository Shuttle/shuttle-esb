using System;
using System.Threading;
using System.Transactions;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DefaultTransactionScope : ITransactionScope
	{
		private readonly bool ignore = false;
        private readonly string name;
        private readonly TransactionScope scope;

        private const IsolationLevel DefaultIsolationLevel = IsolationLevel.ReadUncommitted;
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

		private readonly ILog log;

        public DefaultTransactionScope()
            : this(Guid.NewGuid().ToString("n"), DefaultIsolationLevel, TimeSpan.FromMinutes(15))
        {
        }

        public DefaultTransactionScope(IsolationLevel isolationLevel, TimeSpan timeout)
            : this(Guid.NewGuid().ToString("n"), isolationLevel, timeout)
        {
        }

        public DefaultTransactionScope(string name)
            : this(name, DefaultIsolationLevel, DefaultTimeout)
        {
        }

        public DefaultTransactionScope(string name, IsolationLevel isolationLevel, TimeSpan timeout)
        {
            this.name = name;
        	log = Log.For(this);

        	ignore = Transaction.Current != null;

			if (ignore)
			{
                if (log.IsVerboseEnabled)
                {
                    log.Verbose(string.Format(ESBResources.VerboseTransactionScopeAmbient, name,
                                              Thread.CurrentThread.ManagedThreadId));
                }

			    return;
			}

        	scope = new TransactionScope(TransactionScopeOption.RequiresNew,
				                             new TransactionOptions
				                             	{
				                             		IsolationLevel = isolationLevel,
				                             		Timeout = timeout
				                             	});

            if (log.IsVerboseEnabled)
            {
                log.Verbose(string.Format(ESBResources.VerboseTransactionScopeCreated, name, isolationLevel, timeout,
                                          Thread.CurrentThread.ManagedThreadId));
            }
        }

        public void Dispose()
        {
            if (scope == null)
            {
                return;
            }

        	try
            {
                scope.Dispose();
			}
            catch 
            {
                // ignore --- may be bug in transaction scope: http://connect.microsoft.com/VisualStudio/feedback/details/449469/transactedconnectionpool-bug-in-vista-server-2008-sp2#details
            }
		}

        public void Complete()
        {
			if (ignore)
			{
                if (log.IsVerboseEnabled)
                {
                    log.Verbose(string.Format(ESBResources.VerboseTransactionScopeAmbientCompleted, name,
                                              Thread.CurrentThread.ManagedThreadId));
                }

			    return;
			}

			if (scope == null)
            {
                return;
            }

            scope.Complete();

            if (log.IsVerboseEnabled)
            {
                log.Verbose(string.Format(ESBResources.VerboseTransactionScopeCompleted, name,
                                          Thread.CurrentThread.ManagedThreadId));
            }
        }
    }
}