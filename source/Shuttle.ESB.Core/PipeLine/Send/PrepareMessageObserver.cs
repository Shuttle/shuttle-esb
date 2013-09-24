using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class PrepareMessageObserver : IPipelineObserver<OnPrepareMessage>
	{
		public void Execute(OnPrepareMessage pipelineEvent)
		{
			var transportMessage = pipelineEvent.GetTransportMessage() ?? pipelineEvent.GetServiceBus().CreateTransportMessage(pipelineEvent.GetMessage());

			transportMessage.Headers.Merge(pipelineEvent.GetServiceBus().OutgoingHeaders);

			pipelineEvent.SetTransportMessage(transportMessage);
		}
	}
}