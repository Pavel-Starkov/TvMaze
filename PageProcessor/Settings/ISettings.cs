namespace PageProcessor.Settings
{
    public interface ISettings
    {
        int RetryTimes { get; }
        int DelayInSeconds { get; }
        string LiteDbConnectionString { get; }
        string CosmosDbConnectionString { get; }
        string ShowsApiUrl { get; }
        string ShowCastApiUrl { get; }
        string DateTimeFormat { get; }
    }
}
