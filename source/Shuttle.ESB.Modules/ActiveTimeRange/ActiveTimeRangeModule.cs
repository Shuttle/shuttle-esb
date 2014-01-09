using System;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Modules
{
	public class ActiveTimeRangeModule : IModule, IDisposable, IActiveState
	{
		private volatile bool active;
		private readonly string startupPipelineName = typeof (StartupPipeline).FullName;
		
		public void Initialize(IServiceBus bus)
		{
			Guard.AgainstNull(bus,"bus");

			bus.Events.PipelineCreated += PipelineCreated;
		}

		private void PipelineCreated(object sender, PipelineEventArgs e)
		{
			if (e.Pipeline.GetType().FullName.Equals(startupPipelineName, StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}

			e.Pipeline.RegisterObserver(new ActiveTimeRangeObserver(this));
		}

		public void Dispose()
		{
			active = false;
		}

		public bool Active
		{
			get { return active; }
		}
	}
}