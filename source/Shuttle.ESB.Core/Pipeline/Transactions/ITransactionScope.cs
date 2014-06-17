using System;

namespace Shuttle.ESB.Core
{
	public interface ITransactionScope : IDisposable
	{
		void Complete();
	}
}