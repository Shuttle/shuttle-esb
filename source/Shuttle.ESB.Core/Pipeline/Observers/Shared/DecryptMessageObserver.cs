using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DecryptMessageObserver : IPipelineObserver<OnDecryptMessage>
	{
		public void Execute(OnDecryptMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var transportMessage = state.GetTransportMessage();

			if (!transportMessage.EncryptionEnabled())
			{
				return;
			}

			var algorithm =
				state.GetServiceBus().Configuration.FindEncryptionAlgorithm(transportMessage.EncryptionAlgorithm);

			Guard.Against<InvalidOperationException>(algorithm == null, string.Format(ESBResources.EncryptionAlgorithmException, transportMessage.CompressionAlgorithm));

			transportMessage.Message = algorithm.Decrypt(transportMessage.Message);
		}
	}
}