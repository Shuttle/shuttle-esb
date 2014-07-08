using System;

namespace Shuttle.ESB.Test.Shared
{
    [Serializable]
    public class SimpleMessage : object
    {
        public string Name { get; set; }
    }
}
