using System;
using BugTracker.Providers;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace BugTracker.Extensions
{
    public static class HttpClientBuilderExtension
    {
        public static IHttpClientBuilder AddRetryPolicy(this IHttpClientBuilder clientBuilder)
        {
            // Default settings
            const int retryCount = DefaultRetryPolicySettings.RetryCount;
            var medianFirstRetryDelay =
                TimeSpan.FromMilliseconds(DefaultRetryPolicySettings.MedianFirstRetryDelayInMilliseconds);
            var sleepProvider = new SleepDurationJitterProvider(retryCount, medianFirstRetryDelay);

            return clientBuilder.AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(sleepProvider.Durations));
        }

        public static IHttpClientBuilder AddRetryPolicy(this IHttpClientBuilder clientBuilder,
            ISleepDurationProvider sleepProvider)
        {
            return clientBuilder.AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(sleepProvider.Durations));
        }
    }
}