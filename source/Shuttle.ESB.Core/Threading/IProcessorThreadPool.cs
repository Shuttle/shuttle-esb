using System;

namespace Shuttle.ESB.Core
{
    public interface IProcessorThreadPool : IDisposable
    {
        void Pause();
        void Resume();
        IProcessorThreadPool Start();
    }
}