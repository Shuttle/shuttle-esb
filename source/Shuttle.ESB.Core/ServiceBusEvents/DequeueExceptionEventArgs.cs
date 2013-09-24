using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class DequeueExceptionEventArgs : PipelineEventEventArgs
	{
		public IQueue Queue { get; private set; }
		public Exception Exception { get; private set; }

		public DequeueExceptionEventArgs(PipelineEvent pipelineEvent, IQueue queue, Exception exception)
			: base(pipelineEvent)
		{
			Queue = queue;
			Exception = exception;
		}
	}
}