using System.IO;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class QueueStreamEventArgs : PipelineEventEventArgs
	{
		public QueueStreamEventArgs(PipelineEvent pipelineEvent, IQueue queue, Stream stream)
			: base(pipelineEvent)
		{
			Queue = queue;
			TransportMessageStream = stream;
		}

		public IQueue Queue { get; private set; }
		public Stream TransportMessageStream { get; private set; }
	}
}