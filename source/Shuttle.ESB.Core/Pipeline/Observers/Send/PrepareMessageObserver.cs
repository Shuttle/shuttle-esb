namespace Shuttle.ESB.Core
{
	public class PrepareMessageObserver : IPipelineObserver<OnPrepareMessage>
	{
		public void Execute(OnPrepareMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var transportMessage = state.GetTransportMessage() ??
			                       state.GetServiceBus().CreateTransportMessage(state.GetMessage());

			transportMessage.CorrelationId = state.GetServiceBus().OutgoingCorrelationId;
			transportMessage.Headers.Merge(state.GetServiceBus().OutgoingHeaders);
			transportMessage.IgnoreTillDate = state.GetIgnoreTillDate();

			state.SetTransportMessage(transportMessage);
		}
	}
}