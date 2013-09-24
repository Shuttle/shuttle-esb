using System.Reflection;
using System.Transactions;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class TransactionScopeObserver :
		IPipelineObserver<OnStartTransactionScope>,
		IPipelineObserver<OnCompleteTransactionScope>,
		IPipelineObserver<OnDisposeTransactionScope>,
		IPipelineObserver<OnAbortPipeline>,
		IPipelineObserver<OnPipelineException>
	{
		public void Execute(OnAbortPipeline pipelineEvent)
		{
			var scope = pipelineEvent.GetTransactionScope();

			if (scope == null)
			{
				return;
			}

			if (pipelineEvent.GetTransactionComplete())
			{
				scope.Complete();
			}

            scope.Dispose();

			pipelineEvent.SetTransactionScope(null);
		}

		public void Execute(OnCompleteTransactionScope pipelineEvent)
		{
			var scope = pipelineEvent.GetTransactionScope();

			if (scope == null)
			{
				return;
			}

			if (pipelineEvent.Pipeline.Exception == null || pipelineEvent.GetTransactionComplete())
			{
				scope.Complete();
			}
		}

		public void Execute(OnDisposeTransactionScope pipelineEvent)
		{
			var scope = pipelineEvent.GetTransactionScope();

			if (scope == null)
			{
				return;
			}

			scope.Dispose();

			pipelineEvent.SetTransactionScope(null);
		}

		public void Execute(OnStartTransactionScope pipelineEvent)
		{
			var scope = pipelineEvent.GetTransactionScope();

			if (scope != null)
			{
				throw new TransactionException(
					(string.Format(ESBResources.TransactionAlreadyStarted, GetType().FullName, MethodBase.GetCurrentMethod().Name)));
			}

			scope = pipelineEvent.GetServiceBus().Configuration.TransactionScopeFactory.Create(pipelineEvent);

			pipelineEvent.SetTransactionScope(scope);
		}

		public void Execute(OnPipelineException pipelineEvent)
		{
			var scope = pipelineEvent.GetTransactionScope();

			if (scope == null)
			{
				return;
			}

			if (pipelineEvent.GetTransactionComplete())
			{
				scope.Complete();
			}

			scope.Dispose();

			pipelineEvent.SetTransactionScope(null);
		}
	}
}