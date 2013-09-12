using System;

namespace Shuttle.ESB.SqlServer
{
    public class ScriptException : Exception
    {
        public ScriptException(string message)
            : base(message)
        {
        }
    }
}