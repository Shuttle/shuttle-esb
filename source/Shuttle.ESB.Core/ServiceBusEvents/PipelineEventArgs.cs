using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class PipelineEventArgs : EventArgs
	{
		public ObservablePipeline Pipeline { get; private set; }

		public PipelineEventArgs(ObservablePipeline pipeline)
		{
			Pipeline = pipeline;
		}
	}
}