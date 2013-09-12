using System;

namespace Shuttle.ESB.Test.Shared.Mocks
{
    [Serializable]
    public class SimpleMessage : object
    {
        public string Name { get; set; }
    }
}
