using System;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration
{
	public class InboxDeferredModule :
		IModule,
		IPipelineObserver<OnAfterDeserializeTransportMessage>
	{
		public TransportMessage TransportMessage { get; private set; }

		public void Initialize(IServiceBus bus)
		{
			bus.Events.PipelineCreated += PipelineCreated;
		}

		private void PipelineCreated(object sender, PipelineEventArgs e)
		{
			if (
				!e.Pipeline.GetType()
				  .FullName.Equals(typeof (InboxMessagePipeline).FullName, StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}

			e.Pipeline.RegisterObserver(this);
		}

		public void Execute(OnAfterDeserializeTransportMessage pipelineEvent)
		{
			TransportMessage = pipelineEvent.Pipeline.State.GetTransportMessage();
		}
	}
}