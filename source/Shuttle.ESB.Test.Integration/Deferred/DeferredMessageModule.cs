using System;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Integration.Deferred
{
	public class DeferredMessageModule :
		IModule,
		IPipelineObserver<OnAfterHandleMessage>,
		IPipelineObserver<OnAfterProcessDeferredMessage>
	{
		public bool MessageHandled { get; private set; }
		public bool DeferredMessageReturned { get; private set; }

		public void Initialize(IServiceBus bus)
		{
			Guard.AgainstNull(bus, "bus");

			bus.Events.PipelineCreated += PipelineCreated;
		}

		private void PipelineCreated(object sender, PipelineEventArgs e)
		{
			if (!e.Pipeline.GetType().FullName.Equals(typeof(InboxMessagePipeline).FullName, StringComparison.InvariantCultureIgnoreCase)
			    &&
			    !e.Pipeline.GetType().FullName.Equals(typeof(DeferredMessagePipeline).FullName, StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}

			e.Pipeline.RegisterObserver(this);
		}

		public void Execute(OnAfterHandleMessage pipelineEvent)
		{
			MessageHandled = true;
		}

		public void Execute(OnAfterProcessDeferredMessage pipelineEvent)
		{
			DeferredMessageReturned = pipelineEvent.Pipeline.State.GetDeferredMessageReturned();
		}
	}
}