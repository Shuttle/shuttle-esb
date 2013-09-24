using System;

namespace Shuttle.ESB.Core
{
    public class CouldNotResolveKeyException : Exception
    {
        public CouldNotResolveKeyException(Type type, string key)
            : base(string.Format(ESBResources.CouldNotResolveKeyException, type.FullName, key))
        {
        }
    }
}