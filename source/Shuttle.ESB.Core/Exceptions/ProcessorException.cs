using System;

namespace Shuttle.ESB.Core
{
	public class ProcessorException : Exception
	{
		public ProcessorException(string message) : base(message)
		{
		}
	}
}