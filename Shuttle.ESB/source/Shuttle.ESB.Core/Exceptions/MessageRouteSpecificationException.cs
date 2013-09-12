using System;

namespace Shuttle.ESB.Core
{
    public class MessageRouteSpecificationException  : Exception
    {
        public MessageRouteSpecificationException(string message) : base(message)
        {
        }
    }
}