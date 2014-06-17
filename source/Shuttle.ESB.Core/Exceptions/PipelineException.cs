using System;

namespace Shuttle.ESB.Core
{
	public class PipelineException : Exception
	{
		public PipelineException(string message) : base(message)
		{
		}
	}
}