using System;

namespace Shuttle.ESB.Core
{
    public class CryptographyException : Exception
    {
        public CryptographyException(string message) : base(message)
        {
        }
    }
}