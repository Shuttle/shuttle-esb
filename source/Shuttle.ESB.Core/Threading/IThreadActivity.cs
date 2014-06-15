using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public interface IThreadActivity
    {
        void Waiting(IThreadState state);
        void Working();
    }
}