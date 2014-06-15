using System;

namespace Shuttle.ESB.Core
{
    public class ESBConfigurationException : Exception
    {
        public ESBConfigurationException(string message) : base(message)
        {
        }
    }
}