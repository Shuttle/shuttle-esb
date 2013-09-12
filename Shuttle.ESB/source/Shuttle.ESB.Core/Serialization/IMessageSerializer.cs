using System;
using System.IO;

namespace Shuttle.ESB.Core
{
    public interface IMessageSerializer
    {
        Stream Serialize(object message);
        object Deserialize(Type type, Stream stream);
    }
}