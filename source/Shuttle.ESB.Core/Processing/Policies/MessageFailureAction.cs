using System;

namespace Shuttle.ESB.Core
{
    public class MessageFailureAction
    {
        public MessageFailureAction(bool retry, TimeSpan timeSpanToIgnoreRetriedMessage)
        {
            Retry = retry;
            TimeSpanToIgnoreRetriedMessage = timeSpanToIgnoreRetriedMessage;
        }

        public bool Retry { get; private set; }
        public TimeSpan TimeSpanToIgnoreRetriedMessage { get; private set; }
    }
}