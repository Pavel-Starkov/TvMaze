namespace PageProcessor.Http
{
    public interface IHttpResponse
    {
        string ResponseContent { get; }
        int StatusCode { get; }
        bool IsEnd { get; }
        bool IsOk { get; }
    }
}
