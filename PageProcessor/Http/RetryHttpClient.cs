using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PageProcessor.Http
{
    public class RetryHttpClient : IHttpClient
    {
        private readonly int _retryTimes;
        private readonly int _delayInSeconds;

        public RetryHttpClient(int retryTimes, int delayInSeconds)
        {
            _retryTimes = retryTimes;
            _delayInSeconds = delayInSeconds;
        }

        public async Task<IHttpResponse> Get(string url)
        {
            var triesLeft = _retryTimes;
            do
            {
                using (HttpClient client = new HttpClient())
                {
                    var _response = await client.GetAsync(url);
                    var response = new HttpResponse() { StatusCode = (int)_response.StatusCode };
                    response.ResponseContent = response.StatusCode == (int)HttpStatusCode.OK
                        ? await _response.Content.ReadAsStringAsync()
                        : null;

                    if (response.StatusCode != Constants.StatusCodeBusy)
                    {
                        return response;
                    }
                    triesLeft--;
                    await Task.Delay(TimeSpan.FromSeconds(_delayInSeconds));
                }
            } while (triesLeft > 0);

            return new HttpResponse() { StatusCode = Constants.StatusCodeBusy };
        }
    }
}
