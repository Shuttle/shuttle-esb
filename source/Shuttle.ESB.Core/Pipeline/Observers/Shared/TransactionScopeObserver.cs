using System.Reflection;
using System.Transactions;

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
			var state = pipelineEvent.Pipeline.State;
			var scope = state.GetTransactionScope();

			if (scope == null)
			{
				return;
			}

			if (state.GetTransactionComplete())
			{
				scope.Complete();
			}

            scope.Dispose();

			state.SetTransactionScope(null);
		}

		public void Execute(OnCompleteTransactionScope pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var scope = state.GetTransactionScope();

			if (scope == null)
			{
				return;
			}

			if (pipelineEvent.Pipeline.Exception == null || state.GetTransactionComplete())
			{
				scope.Complete();
			}
		}

		public void Execute(OnDisposeTransactionScope pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var scope = state.GetTransactionScope();

			if (scope == null)
			{
				return;
			}

			scope.Dispose();

			state.SetTransactionScope(null);
		}

		public void Execute(OnStartTransactionScope pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var scope = state.GetTransactionScope();

			if (scope != null)
			{
				throw new TransactionException(
					(string.Format(ESBResources.TransactionAlreadyStartedException, GetType().FullName, MethodBase.GetCurrentMethod().Name)));
			}

			scope = state.GetServiceBus().Configuration.TransactionScopeFactory.Create(pipelineEvent);

			state.SetTransactionScope(scope);
		}

		public void Execute(OnPipelineException pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var scope = state.GetTransactionScope();

			if (scope == null)
			{
				return;
			}

			if (state.GetTransactionComplete())
			{
				scope.Complete();
			}

			scope.Dispose();

			state.SetTransactionScope(null);
		}
	}
}