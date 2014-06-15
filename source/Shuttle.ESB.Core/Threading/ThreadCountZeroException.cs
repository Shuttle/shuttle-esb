using System;

namespace Shuttle.ESB.Core
{
	public class ThreadCountZeroException : Exception
	{
		public ThreadCountZeroException()
			: base(ESBResources.ThreadCountZeroException)
		{
		}
	}
}