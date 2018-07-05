using System.Threading.Tasks;
using PageProcessor.Models;

namespace PageProcessor.Storage
{
    public interface IStorage
    {
        Task AddOrUpdate(IShowsPage page);
        Task<IShowsPage> GetPageByPageId(long pageId);
    }
}
