using System;

namespace Shuttle.ESB.Core
{
    public class SendMessageException : Exception
    {
        public SendMessageException(string message) : base(message)
        {
        }
    }
}