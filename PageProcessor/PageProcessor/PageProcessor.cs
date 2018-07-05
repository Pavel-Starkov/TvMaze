using System.Collections.Generic;
using System.Threading.Tasks;
using PageProcessor.Http;
using PageProcessor.JsonConverter;
using PageProcessor.Models;
using PageProcessor.ServiceFactory;
using PageProcessor.Storage;

namespace PageProcessor.PageProcessor
{
    public class PageProcessor : IPageProcessor
    {
       
        private readonly IServiceFactory<IHttpClient> _httpClientFactory;
        private readonly IServiceFactory<IJsonConverter> _jsonConverterFactory;
        private readonly IServiceFactory<IStorage> _storageFactory;
        private readonly string _showsApiUrl;
        private readonly string _showCastApiUrl;

        public PageProcessor(IServiceFactory<IHttpClient> httpClientFactory, 
                             IServiceFactory<IJsonConverter> jsonConverterFactory, 
                             IServiceFactory<IStorage> storageFactory,
                             string showsApiUrl,
                             string showCastApiUrl)
        {
            
            _httpClientFactory = httpClientFactory;
            _jsonConverterFactory = jsonConverterFactory;
            _storageFactory = storageFactory;
            _showsApiUrl = showsApiUrl;
            _showCastApiUrl = showCastApiUrl;
        }

        public async Task<bool> ProcessPage(long pageId)
        {
            var client = _httpClientFactory.Service;

            var pageUrl = _showsApiUrl.Replace(Constants.PageIdToken, pageId.ToString());
            var response = await client.Get(pageUrl);

            if (response.IsEnd) return false;

            if (response.IsOk)
            {
                var shows = _jsonConverterFactory.Service.DeserializeObject<List<ShowCastModel>>(response.ResponseContent);
                foreach (var show in shows)
                {
                    var castUrl = _showCastApiUrl.Replace(Constants.ShowIdToken, show.id);
                    var castResponse = await client.Get(castUrl);
                    if (castResponse.IsOk)
                    {
                        show.sourceCast = _jsonConverterFactory.Service.DeserializeObject<List<ShowCastModel.Person>>(castResponse.ResponseContent);
                    }
                }

                var showsJson = _jsonConverterFactory.Service.SerializeObject(shows);

                var showsPage = new ShowsPage{ Id = pageId, Json = showsJson };

                await _storageFactory.Service.AddOrUpdate(showsPage);
            }

            return true;
        }
    }
}
