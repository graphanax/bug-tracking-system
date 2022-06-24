namespace BugTracker.Providers
{
    public static class DefaultRetryPolicySettings
    {
        public const int RetryCount = 3;
        public const int MedianFirstRetryDelayInMilliseconds = 2000;
    }
}