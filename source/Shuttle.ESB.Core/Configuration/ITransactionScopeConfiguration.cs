using System.Transactions;

namespace Shuttle.ESB.Core
{
	public interface ITransactionScopeConfiguration
	{
		bool Enabled { get; }
		IsolationLevel IsolationLevel { get; }
		int TimeoutSeconds { get; }
	}
}