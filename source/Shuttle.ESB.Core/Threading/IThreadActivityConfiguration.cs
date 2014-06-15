using System;

namespace Shuttle.ESB.Core
{
    public interface IThreadActivityConfiguration
    {
        TimeSpan[] DurationToSleepWhenIdle { get; set; }
    }
}