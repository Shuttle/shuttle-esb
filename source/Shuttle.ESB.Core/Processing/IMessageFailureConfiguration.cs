using System;

namespace Shuttle.ESB.Core
{
    public interface IMessageFailureConfiguration
    {
        int MaximumFailureCount { get; set; }
        TimeSpan[] DurationToIgnoreOnFailure { get; set; }
    }
}