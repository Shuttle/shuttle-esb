using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class EncryptMessageObserver : IPipelineObserver<OnEncryptMessage>
	{
		public void Execute(OnEncryptMessage pipelineEvent)
		{
			var transportMessage = pipelineEvent.GetTransportMessage();

			if (!transportMessage.EncryptionEnabled())
			{
				return;
			}

			var algorithm =
				pipelineEvent.GetServiceBus().Configuration.FindEncryptionAlgorithm(transportMessage.EncryptionAlgorithm);

			Guard.Against<InvalidOperationException>(algorithm == null, string.Format(Resources.EncryptionAlgorithmException, transportMessage.CompressionAlgorithm));

			transportMessage.Message = algorithm.Encrypt(transportMessage.Message);
		}
	}
}