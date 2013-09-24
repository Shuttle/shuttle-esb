using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Unit.Core
{
	public class MockModule : IModule
	{
		private readonly ServiceBusTest test;

		public MockModule(ServiceBusTest test)
		{
			this.test = test;
		}

		public void Initialize(IServiceBus bus)
		{
            bus.Events.AfterTransportMessageDeserialization += AfterTransportMessageDeserialization;
		}

        private void AfterTransportMessageDeserialization(object sender, TransportMessageSerializationEventArgs e)
		{
			test.TransportMessage = e.TransportMessage;
		}
	}
}