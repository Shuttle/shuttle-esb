using System;

namespace Shuttle.ESB.Core
{
    public class RemoveMessageException : Exception
    {
			public RemoveMessageException(string message)
				: base(message)
        {
        }
    }
}