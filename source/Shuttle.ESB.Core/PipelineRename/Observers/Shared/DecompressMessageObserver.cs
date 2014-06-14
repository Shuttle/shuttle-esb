using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DecompressMessageObserver : IPipelineObserver<OnDecompressMessage>
	{
		public void Execute(OnDecompressMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var transportMessage = state.GetTransportMessage();

			if (!transportMessage.CompressionEnabled())
			{
				return;
			}

			var algorithm =
				state.GetServiceBus().Configuration.FindCompressionAlgorithm(transportMessage.CompressionAlgorithm);

			Guard.Against<InvalidOperationException>(algorithm == null, string.Format(ESBResources.CompressionAlgorithmException, transportMessage.CompressionAlgorithm));

			transportMessage.Message = algorithm.Decompress(transportMessage.Message);
		}
	}
}