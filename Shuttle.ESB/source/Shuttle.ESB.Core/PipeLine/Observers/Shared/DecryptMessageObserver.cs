using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DecryptMessageObserver : IPipelineObserver<OnDecryptMessage>
	{
		public void Execute(OnDecryptMessage pipelineEvent)
		{
			var transportMessage = pipelineEvent.GetTransportMessage();

			if (!transportMessage.EncryptionEnabled())
			{
				return;
			}

			var algorithm =
				pipelineEvent.GetServiceBus().Configuration.FindEncryptionAlgorithm(transportMessage.EncryptionAlgorithm);

			Guard.Against<InvalidOperationException>(algorithm == null, string.Format(ESBResources.EncryptionAlgorithmException, transportMessage.CompressionAlgorithm));

			transportMessage.Message = algorithm.Decrypt(transportMessage.Message);
		}
	}
}