using System;

namespace Shuttle.ESB.Core
{
    public class EnqueueMessageDataTypeMismatchException : Exception
    {
        public EnqueueMessageDataTypeMismatchException(string incorrectTypeName, string uri, string expectedTypeName)
            : base(string.Format(ESBResources.EnqueueMessageDataTypeMismatchException, incorrectTypeName, uri, expectedTypeName))
        {
        }
    }
}