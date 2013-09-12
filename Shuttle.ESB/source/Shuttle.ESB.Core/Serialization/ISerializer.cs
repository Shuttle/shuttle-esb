using System;
using System.IO;

namespace Shuttle.ESB.Core
{
    public interface ISerializer
    {
        Stream Serialize(object message);
        object Deserialize(Type type, Stream stream);
    }
}