using System;

namespace Shuttle.ESB.Core
{
    public class ThreadStateEventArgs : EventArgs
    {
        public Type PipelineType { get; private set; }

        public ThreadStateEventArgs(Type pipelineType)
        {
            PipelineType = pipelineType;
        }
    }
}