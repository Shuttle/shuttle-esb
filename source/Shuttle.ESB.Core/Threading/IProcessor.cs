using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
    public interface IProcessor
    {
        void Execute(IThreadState state);
    }
}