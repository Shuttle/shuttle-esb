using System.Collections.Generic;
using System.IO;

namespace Shuttle.ESB.Core
{
    public interface IQueueReader
    {
        IEnumerable<Stream> Read(int top);
    }
}