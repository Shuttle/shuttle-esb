namespace Shuttle.ESB.Core
{
	public class NullTransactionScope : ITransactionScope
	{
		public void Complete()
		{
		}

		public void Dispose()
		{
		}
	}
}