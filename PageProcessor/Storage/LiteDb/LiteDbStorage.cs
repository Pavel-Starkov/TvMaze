using System.Threading.Tasks;
using LiteDB;
using PageProcessor.Models;

namespace PageProcessor.Storage.LiteDb
{
    public class LiteDbStorage : IStorage
    {
        private readonly string _connectionString;
        public LiteDbStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task AddOrUpdate(IShowsPage page)
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var col = db.GetCollection<ShowsPage>(Constants.DbCollectionName);
                    col?.Upsert(page.Id, page as ShowsPage ?? new ShowsPage{Id = page.Id, Json = page.Json});
                }
            });
        }

        public Task<IShowsPage> GetPageByPageId(long pageId)
        {
            return Task.Run(() =>
            {

                using (var db = new LiteDatabase(_connectionString))
                {
                    var col = db.GetCollection<ShowsPage>(Constants.DbCollectionName);
                    return col?.FindById(pageId) as IShowsPage;
                }
            });
        }
    }
}
