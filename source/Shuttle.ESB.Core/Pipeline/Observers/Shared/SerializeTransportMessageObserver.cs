using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class SerializeTransportMessageObserver : IPipelineObserver<OnSerializeTransportMessage>
	{
		public void Execute(OnSerializeTransportMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var messageSenderContext = state.GetMessageSenderContext();

			Guard.AgainstNull(messageSenderContext, "messageSenderContext");

			state.SetTransportMessageStream(state.GetServiceBus().Configuration.Serializer.Serialize(messageSenderContext.TransportMessage));
		}
	}
}