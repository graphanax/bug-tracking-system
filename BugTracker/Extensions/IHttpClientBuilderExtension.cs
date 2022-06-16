using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace BugTracker.Extensions
{
    public static class HttpClientBuilderExtension
    {
        public static IHttpClientBuilder AddRetryPolicy(this IHttpClientBuilder clientBuilder)
        {
            var maxDelay = TimeSpan.FromSeconds(45);
            var delay = Backoff
                .DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 50)
                .Select(s => TimeSpan.FromTicks(Math.Min(s.Ticks, maxDelay.Ticks)));

            return clientBuilder.AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(delay));
        }
    }
}