using System.Threading.Tasks;

namespace PageProcessor.Http
{
    public interface IHttpClient
    {
        Task<IHttpResponse> Get(string url);
    }
}
