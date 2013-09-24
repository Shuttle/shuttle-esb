namespace Shuttle.ESB.Core
{
    public interface IIdempotenceTracker
    {
        bool Contains(TransportMessage transportMessage);
        void Add(TransportMessage transportMessage);
        void Remove(TransportMessage transportMessage);
    }
}