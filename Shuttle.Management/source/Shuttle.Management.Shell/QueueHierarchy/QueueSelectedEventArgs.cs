using System;

namespace Shuttle.Management.Shell
{
    public class QueueSelectedEventArgs : EventArgs
    {
        public QueueSelectedEventArgs(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; private set; }
    }
}