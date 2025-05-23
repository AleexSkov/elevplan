using MongoDB.Driver;
using Core.Db;
using Core.Models;

namespace ServerApi.Repository
{
    public class ElevplanRepository
    {
        private readonly IMongoCollection<Elevplan> _collection;

        public ElevplanRepository(MongoDbContext context)
        {
            _collection = context.GetCollection<Elevplan>("elevplan");
        }

        public async Task<List<Elevplan>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
    }
}