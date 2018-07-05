namespace PageProcessor.ServiceFactory
{
    public interface IServiceFactory<out TService>
    {
        TService Service { get; }
    }
}
