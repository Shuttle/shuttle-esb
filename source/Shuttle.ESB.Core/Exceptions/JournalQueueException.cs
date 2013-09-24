using System;

namespace Shuttle.ESB.Core
{
    public class JournalQueueException : Exception
    {
        public JournalQueueException(Uri uri)
            : base(string.Format(ESBResources.JournalQueueException, uri))
        {
        }
    }
}