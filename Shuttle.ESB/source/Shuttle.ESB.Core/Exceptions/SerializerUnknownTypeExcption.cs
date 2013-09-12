using System;

namespace Shuttle.ESB.Core
{
    public class SerializerUnknownTypeExcption : Exception
    {
        public SerializerUnknownTypeExcption(string type)
            : base(string.Format(ESBResources.SerializerUnknownTypeExcption, type))
        {
        }
    }
}