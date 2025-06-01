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
            // Initialiserer forbindelse til MongoDB ved hjælp af URI
            var mongoUri = "mongodb+srv://benjaminlorenzen:pdx45bjd@" +
                           "cluster0.55cag.mongodb.net/Comwell?"  +
                           "retryWrites=true&w=majority";

            var client = new MongoClient(mongoUri);
            var db     = client.GetDatabase("Comwell");

            // Henter samlingen af AppUsers fra databasen
            _appUserCollection = db.GetCollection<AppUser>("AppUsers");
        }

        // Henter alle brugere fra databasen
        public async Task<List<AppUser>> GetAllAsync()
            => await _appUserCollection.Find(_ => true).ToListAsync();

        // Finder bruger baseret på e-mail
        public async Task<AppUser?> GetByEmailAsync(string email)
            => await _appUserCollection.Find(u => u.Email == email)
                .FirstOrDefaultAsync();

        // Finder bruger baseret på ID
        public async Task<AppUser?> GetByIdAsync(ObjectId id)
            => await _appUserCollection.Find(u => u.Id == id)
                .FirstOrDefaultAsync();

        // Opretter en ny bruger i databasen
        public async Task CreateAsync(AppUser user)
        {
            user.CreatedAt = DateTime.UtcNow; // Sætter oprettelsesdato
            await _appUserCollection.InsertOneAsync(user);
        }

        // Tjekker om en e-mail allerede findes i databasen
        public async Task<bool> EmailExistsAsync(string email)
            => await _appUserCollection.Find(u => u.Email == email)
                .AnyAsync();

        // Opdaterer eksisterende bruger med nyt indhold
        public async Task UpdateAsync(ObjectId id, AppUser updatedUser)
        {
            var filter = Builders<AppUser>.Filter.Eq(u => u.Id, id);
            await _appUserCollection.ReplaceOneAsync(filter, updatedUser);
        }
    }
}
