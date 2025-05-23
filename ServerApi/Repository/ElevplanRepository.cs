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

        public ElevplanRepository()
        {
            var mongoUri = "mongodb+srv://benjaminlorenzen:pdx45bjd@cluster0.55cag.mongodb.net/Comwell?retryWrites=true&w=majority";
            var client = new MongoClient(mongoUri);
            var database = client.GetDatabase("Comwell");
            _elevplanCollection = database.GetCollection<Elevplan>("Elevplan");
        }

        public async Task<List<Elevplan>> GetAllAsync()
        {
            return await _elevplanCollection.Find(_ => true).ToListAsync();
        }
    }
}