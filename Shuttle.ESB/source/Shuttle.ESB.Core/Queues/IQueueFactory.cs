using System;

namespace Shuttle.ESB.Core
{
    public interface IQueueFactory
    {
        string Scheme { get; }
        IQueue Create(Uri uri);
        bool CanCreate(Uri uri);
    }
}