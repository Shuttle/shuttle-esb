using System;

namespace Shuttle.ESB.Core
{
    public class CouldNotResolveTypeException : Exception
    {
        public CouldNotResolveTypeException(Type type)
            : base(string.Format(ESBResources.CouldNotResolveTypeException, type.FullName))
        {
        }
    }
}