using System;

namespace Shuttle.ESB.Core
{
	public class PipelineEventArgs : EventArgs
	{
		public Pipeline Pipeline { get; private set; }

		public PipelineEventArgs(Pipeline pipeline)
		{
			Pipeline = pipeline;
		}
	}
}