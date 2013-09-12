using System;

namespace Shuttle.ESB.Core
{
    public class QueueConfigurationException : Exception
    {
        public QueueConfigurationException(string message) : base(message)
        {
        }
    }
}