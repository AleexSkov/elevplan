using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Core.Models;
using ServerApi.Interface;

namespace ServerApi.Repository
{
    public class ElevplanRepository : IElevplan
    {
        private readonly IMongoCollection<Elevplan> _elevplanCollection;

        public ElevplanRepository(IMongoCollection<Elevplan> elevplanCollection)
        {
            _elevplanCollection = elevplanCollection;
        }

        public async Task<List<Elevplan>> GetAllAsync()
        {
            return await _elevplanCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Elevplan?> GetTemplateAsync()
        {
            // Hent template-dokumentet med elev_id = "TBD"
            var filter = Builders<Elevplan>.Filter.Eq(p => p.ElevId, "TBD");
            return await _elevplanCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Elevplan plan)
        {
            // Generer nyt unikt int Id i repository
            var maxIdDoc = await _elevplanCollection
                .Find(_ => true)
                .SortByDescending(p => p.Id)
                .Limit(1)
                .FirstOrDefaultAsync();

            plan.Id = (maxIdDoc?.Id ?? 0) + 1;
            await _elevplanCollection.InsertOneAsync(plan);
        }
    }
}