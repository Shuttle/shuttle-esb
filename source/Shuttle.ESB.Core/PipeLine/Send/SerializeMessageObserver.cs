using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class SerializeMessageObserver : IPipelineObserver<OnSerializeMessage>
	{
		public void Execute(OnSerializeMessage pipelineEvent)
		{
			var message = pipelineEvent.GetMessage();
			var transportMessage = pipelineEvent.GetTransportMessage();

			var bytes = pipelineEvent.GetServiceBus()
				.Configuration.MessageSerializer
				.Serialize(message).ToBytes();

			pipelineEvent.SetMessageBytes(bytes);

			transportMessage.Message = bytes;
		}
	}
}