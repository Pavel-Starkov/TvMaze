using System.Threading.Tasks;

namespace PageProcessor.PageProcessor
{
    public interface IPageProcessor
    {
        Task<bool> ProcessPage(long pageId);
    }
}
