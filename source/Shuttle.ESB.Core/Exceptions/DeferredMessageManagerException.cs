using System;

namespace Shuttle.ESB.Core
{
    public class DeferredMessageManagerException : Exception
    {
		public DeferredMessageManagerException(string message)
			: base(message)
        {
        }
    }
}