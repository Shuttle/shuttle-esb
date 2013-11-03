namespace Shuttle.ESB.Core
{
	public class NullServiceBusTransactionScope : IServiceBusTransactionScope
	{
		public void Complete()
		{
		}

		public void Dispose()
		{
		}
	}
}