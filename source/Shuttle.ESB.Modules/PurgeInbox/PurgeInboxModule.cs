using System;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Modules.PurgeInbox
{
	public class PurgeInboxModule : IModule
	{
		private IServiceBus _bus;

		private readonly string startupPipelineName = typeof(StartupPipeline).FullName;

		public void Initialize(IServiceBus bus)
		{
			_bus = bus;

			_bus.Events.PipelineCreated += PipelineCreated;
		}

		private void PipelineCreated(object sender, PipelineEventArgs e)
		{
			if (!e.Pipeline.GetType().FullName.Equals(startupPipelineName, StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}

			e.Pipeline.RegisterObserver(new PurgeInboxObserver());
		}
	}
}