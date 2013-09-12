using System;

namespace Shuttle.ESB.Core
{
    public class MessageDeserializationException : Exception
    {
        public MessageDeserializationException(Exception exception)
            : base(Resources.MessageDeserializationException, exception)
        {
        }
    }
}