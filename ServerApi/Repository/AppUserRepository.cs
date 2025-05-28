using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Core.Models;
using ServerApi.Interface;

namespace ServerApi.Repository
{
    public class AppUserRepository : IAppUser
    {
        private readonly IMongoCollection<AppUser> _appUserCollection;

        public AppUserRepository()
        {
            // Samme connection string–flow her
            var mongoUri = 
                "mongodb+srv://benjaminlorenzen:pdx45bjd@" +
                "cluster0.55cag.mongodb.net/Comwell?" +
                "retryWrites=true&w=majority";
            var client   = new MongoClient(mongoUri);
            var db       = client.GetDatabase("Comwell");
            _appUserCollection = db.GetCollection<AppUser>("AppUsers");
        }

        public async Task<List<AppUser>> GetAllAsync()
            => await _appUserCollection.Find(_ => true).ToListAsync();

        public async Task<AppUser?> GetByEmailAsync(string email)
            => await _appUserCollection
                .Find(u => u.Email == email)
                .FirstOrDefaultAsync();

        public async Task<AppUser?> GetByIdAsync(ObjectId id)
            => await _appUserCollection
                .Find(u => u.Id == id)
                .FirstOrDefaultAsync();

        public async Task CreateAsync(AppUser user)
        {
            user.CreatedAt = DateTime.UtcNow;
            await _appUserCollection.InsertOneAsync(user);
        }

        public async Task<bool> EmailExistsAsync(string email)
            => await _appUserCollection
                .Find(u => u.Email == email)
                .AnyAsync();

        public async Task UpdateAsync(ObjectId id, AppUser updatedUser)
        {
            var filter = Builders<AppUser>
                .Filter.Eq(u => u.Id, id);
            await _appUserCollection
                .ReplaceOneAsync(filter, updatedUser);
        }
    }
}