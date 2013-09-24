using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public class SerializeTransportMessageObserver : IPipelineObserver<OnSerializeTransportMessage>
    {
        public void Execute(OnSerializeTransportMessage pipelineEvent)
        {
            var transportMessage = pipelineEvent.GetTransportMessage();

            pipelineEvent.SetTransportMessageStream(pipelineEvent.GetServiceBus()
                                                        .Configuration.Serializer
                                                        .Serialize(transportMessage));

            pipelineEvent.GetServiceBus().Events.OnAfterTransportMessageSerialization(
                this,
                new TransportMessageSerializationEventArgs(pipelineEvent, transportMessage));
        }
    }
}