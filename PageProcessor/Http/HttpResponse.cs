using System.Net;

namespace PageProcessor.Http
{
    public class HttpResponse : IHttpResponse
    {
        public string ResponseContent { get; set; }
        public int StatusCode { get; set; }
        public bool IsEnd => StatusCode == (int)HttpStatusCode.NotFound;
        public bool IsOk => StatusCode == (int)HttpStatusCode.OK;
    }
}
