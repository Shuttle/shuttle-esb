using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class PipelineExceptionEventArgs : EventArgs
	{
		public ObservablePipeline Pipeline { get; private set; }

		public PipelineExceptionEventArgs(ObservablePipeline pipeline)
		{
			Pipeline = pipeline;
		}
	}
}