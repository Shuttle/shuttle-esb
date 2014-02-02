using System;

namespace Shuttle.ESB.Core
{
	public class TransportMessageStateFileTransaction : IDisposable
	{
		private readonly TransportMessageStateFile _transportMessageStateFile;

		public TransportMessageStateFileTransaction(TransportMessageStateFile transportMessageStateFile)
		{
			_transportMessageStateFile = transportMessageStateFile;
		}

		public void Complete()
		{
			_transportMessageStateFile.CommitTransaction();
		}

		public void Dispose()
		{
			_transportMessageStateFile.ProcessTransaction();
		}
	}
}