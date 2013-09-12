using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DecompressMessageObserver : IPipelineObserver<OnDecompressMessage>
	{
		public void Execute(OnDecompressMessage pipelineEvent)
		{
			var transportMessage = pipelineEvent.GetTransportMessage();

			if (!transportMessage.CompressionEnabled())
			{
				return;
			}

			var algorithm =
				pipelineEvent.GetServiceBus().Configuration.FindCompressionAlgorithm(transportMessage.CompressionAlgorithm);

			Guard.Against<InvalidOperationException>(algorithm == null, string.Format(ESBResources.CompressionAlgorithmException, transportMessage.CompressionAlgorithm));

			transportMessage.Message = algorithm.Decompress(transportMessage.Message);
		}
	}
}