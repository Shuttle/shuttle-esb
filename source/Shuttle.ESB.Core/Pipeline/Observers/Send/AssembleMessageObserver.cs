using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class AssembleMessageObserver : IPipelineObserver<OnAssembleMessage>
	{
		public void Execute(OnAssembleMessage pipelineEvent)
		{
			var state = pipelineEvent.Pipeline.State;
			var transportMessageConfigurator = state.Get<TransportMessageConfigurator>(StateKeys.TransportMessageConfigurator);
			var bus = state.GetServiceBus();

			Guard.AgainstNull(transportMessageConfigurator, "transportMessageConfigurator");
			Guard.AgainstNull(transportMessageConfigurator.Message, "transportMessageConfigurator.Message");

			state.SetTransportMessage(transportMessageConfigurator.TransportMessage(bus.Configuration));
			state.SetMessage(transportMessageConfigurator.Message);
		}
	}
}