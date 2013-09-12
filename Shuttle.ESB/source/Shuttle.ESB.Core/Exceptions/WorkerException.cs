using System;

namespace Shuttle.ESB.Core
{
    public class WorkerException : Exception
    {
        public WorkerException(string message) : base(message)
        {
        }
    }
}