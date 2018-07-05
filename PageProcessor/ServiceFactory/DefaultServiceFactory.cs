using System;
using PageProcessor.Http;
using PageProcessor.JsonConverter;
using PageProcessor.PageProcessor;
using PageProcessor.Settings;
using PageProcessor.Storage;
using PageProcessor.Storage.CosmosDb;

namespace PageProcessor.ServiceFactory
{
    public class DefaultServiceFactory : IServiceFactory<IStorage>, IServiceFactory<IHttpClient>, IServiceFactory<IJsonConverter>, IServiceFactory<IPageProcessor>
    {
        private readonly Lazy<IStorage> _storage;
        private readonly Lazy<IHttpClient> _httpClient;
        private readonly Lazy<IJsonConverter> _jsonConverter;
        private readonly Lazy<IPageProcessor> _pageProcessor;

        public DefaultServiceFactory()
        {
            var settings = new Lazy<ISettings>(() => new DefaultSettings().SetFromConfig());

            _storage = new Lazy<IStorage>(() => new CosmosDbStorage(settings.Value.CosmosDbConnectionString));
            _httpClient = new Lazy<IHttpClient>(() => new RetryHttpClient(settings.Value.RetryTimes, settings.Value.DelayInSeconds));
            _jsonConverter = new Lazy<IJsonConverter>(() => new NewtonsoftJsonConverter(settings.Value.DateTimeFormat));
            _pageProcessor = new Lazy<IPageProcessor>(() => new PageProcessor.PageProcessor(this, this, this, settings.Value.ShowsApiUrl, settings.Value.ShowCastApiUrl));
        }

        IStorage IServiceFactory<IStorage>.Service => _storage.Value;

        IHttpClient IServiceFactory<IHttpClient>.Service => _httpClient.Value;

        IJsonConverter IServiceFactory<IJsonConverter>.Service => _jsonConverter.Value;

        IPageProcessor IServiceFactory<IPageProcessor>.Service => _pageProcessor.Value;
    }
}
