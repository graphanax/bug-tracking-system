using System;
using System.Collections.Generic;

namespace BugTracker.Providers
{
    public interface ISleepDurationProvider
    {
        int RetryCount { get; }
        IEnumerable<TimeSpan> Durations { get; }
    }
}