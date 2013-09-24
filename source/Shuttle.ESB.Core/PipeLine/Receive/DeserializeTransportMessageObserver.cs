using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DeserializeTransportMessageObserver : IPipelineObserver<OnDeserializeTransportMessage>
	{
		public void Execute(OnDeserializeTransportMessage pipelineEvent)
		{
			Guard.AgainstNull(pipelineEvent.GetTransportMessageStream(), "dequeuedStream");
			Guard.AgainstNull(pipelineEvent.GetWorkQueue(), "workQueue");
			Guard.AgainstNull(pipelineEvent.GetErrorQueue(), "errorQueue");

			TransportMessage transportMessage;

			using (var stream = pipelineEvent.GetTransportMessageStream().Copy())
			{
				transportMessage = (TransportMessage)pipelineEvent.GetServiceBus().Configuration.MessageSerializer.Deserialize(typeof(TransportMessage), stream);
			}

			pipelineEvent.SetTransportMessage(transportMessage);
			pipelineEvent.SetMessageBytes(transportMessage.Message);

			transportMessage.AcceptInvariants();

			Log.Debug(string.Format(Resources.DebugTransportMessageDeserialized, transportMessage.MessageType,
									transportMessage.MessageId));
		}
	}
}