namespace Shuttle.ESB.Core
{
	public class SerializeTransportMessageObserver : IPipelineObserver<OnSerializeTransportMessage>
	{
		public void Execute(OnSerializeTransportMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var transportMessage = state.GetTransportMessage();

			state.SetTransportMessageStream(state.GetServiceBus().Configuration.Serializer.Serialize(transportMessage));
		}
	}
}