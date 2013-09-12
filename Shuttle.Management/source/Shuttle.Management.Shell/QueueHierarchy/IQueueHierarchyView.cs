using System;

namespace Shuttle.Management.Shell
{
    public interface IQueueHierarchyView
    {
        event EventHandler<QueueSelectedEventArgs> QueueSelected;

        void AddQueue(Uri uri);
        void AddQueue(string uri);

        bool RemoveQueue(Uri uri);
        bool RemoveQueue(string uri);

        bool ContainsQueue(Uri uri);
        bool ContainsQueue(string uri);

        void Clear();
    }
}