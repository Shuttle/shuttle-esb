using System;

namespace Shuttle.ESB.Core
{
    public class IdempotenceServiceException : Exception
    {
        public IdempotenceServiceException(string message) : base(message)
        {
        }
    }
}