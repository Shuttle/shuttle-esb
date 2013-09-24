using System;

namespace Shuttle.ESB.Core
{
    public class IdempotenceTrackerException : Exception
    {
        public IdempotenceTrackerException(string message) : base(message)
        {
        }
    }
}