namespace PageProcessor
{
    public static class Constants
    {
        public const string LiteDbConnectionString = "LiteDbConnectionString";
        public const string CosmosDbConnectionString = "CosmosDbConnectionString";
        public const string DelayInSeconds = "DelayInSeconds";
        public const string RetryTimes = "RetryTimes";
        public const string DbCollectionName = "Pages";
        public const string DbName = "Shows";
        public const string PageIdToken = "{PageId}";
        public const string ShowIdToken = "{ShowId}";
        public const string ShowsApiUrl = "ShowsApiUrl";
        public const string ShowCastApiUrl = "ShowCastApiUrl";
        public const string DateTimeFormat = "DateTimeFormat";
        public const int StatusCodeBusy = 429;
    }
}
