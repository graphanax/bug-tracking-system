using System;
using System.Collections.Generic;
using Polly.Contrib.WaitAndRetry;

namespace BugTracker.Providers
{
    public class SleepDurationJitterProvider : ISleepDurationProvider
    {
        public int RetryCount { get; }
        public IEnumerable<TimeSpan> Durations { get; }

        public SleepDurationJitterProvider(int retryCount, TimeSpan medianFirstRetryDelay)
        {
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount), retryCount, "must be >= 0");

            if (medianFirstRetryDelay < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(medianFirstRetryDelay), medianFirstRetryDelay,
                    "must be >= 0ms");

            Durations = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay, retryCount);
            RetryCount = retryCount;
        }
    }
}