using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class SerializeMessageObserver : IPipelineObserver<OnSerializeMessage>
    {
        public void Execute(OnSerializeMessage pipelineEvent)
        {
            var message = pipelineEvent.GetMessage();
            var transportMessage = pipelineEvent.GetTransportMessage();
            var bus = pipelineEvent.GetServiceBus();
            var bytes = pipelineEvent.GetServiceBus()
                .Configuration.Serializer
                .Serialize(message).ToBytes();

            bus.Events.OnAfterMessageSerialization(
                this,
                new MessageSerializationEventArgs(pipelineEvent, transportMessage, message));

            pipelineEvent.SetMessageBytes(bytes);

            transportMessage.Message = bytes;
        }
    }
}