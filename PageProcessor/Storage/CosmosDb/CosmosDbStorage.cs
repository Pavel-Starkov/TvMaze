using System.Threading.Tasks;
using MongoDB.Driver;
using PageProcessor.Models;

namespace PageProcessor.Storage.CosmosDb
{
    public class CosmosDbStorage : IStorage
    {
        private readonly string _connectionString;
        public CosmosDbStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task  AddOrUpdate(IShowsPage page)
        {
            return Collection.ReplaceOneAsync(
                p => p.Id == page.Id,
                page as ShowsPage ?? new ShowsPage { Id = page.Id, Json = page.Json },
                new UpdateOptions { IsUpsert = true });
        }

        public async Task<IShowsPage> GetPageByPageId(long pageId)
        {
            var page =  await (await Collection.FindAsync(p => p.Id == pageId)).FirstOrDefaultAsync();
            return page;
        }

        private IMongoCollection<ShowsPage> Collection => new MongoClient(_connectionString).GetDatabase(Constants.DbName).GetCollection<ShowsPage>(Constants.DbCollectionName);
        
    }
}
