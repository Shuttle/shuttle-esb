using System;

namespace Shuttle.ESB.Core
{
    public class DeferredMessageException : Exception
    {
		public DeferredMessageException(string message)
			: base(message)
        {
        }
    }
}