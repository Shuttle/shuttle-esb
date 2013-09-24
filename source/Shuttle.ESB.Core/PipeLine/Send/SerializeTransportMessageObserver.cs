using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class SerializeTransportMessageObserver : IPipelineObserver<OnSerializeTransportMessage>
	{
		public void Execute(OnSerializeTransportMessage pipelineEvent)
		{
			pipelineEvent.SetTransportMessageStream(pipelineEvent.GetServiceBus()
				.Configuration.MessageSerializer
				.Serialize(pipelineEvent.GetTransportMessage()));
		}
	}
}