using System;

namespace Shuttle.ESB.Core
{
	public interface IServiceBusTransactionScope : IDisposable
	{
		void Complete();
	}
}