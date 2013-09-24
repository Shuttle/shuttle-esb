using System;

namespace Shuttle.ESB.Core
{
	public class ProcessMessageMethodMissingException : Exception
	{
		public ProcessMessageMethodMissingException(string message) : base(message)
		{
		}
	}
}