using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Core.Models;
using Core.Interface;

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

        public async Task<Elevplan?> GetByElevIdAsync(string elevId)
        {
            return await _elevplanCollection.Find(e => e.elev_id == elevId).FirstOrDefaultAsync();
        }

        public async Task UpdateOpgaveAsync(string elevId, int periodeNummer, string kategori, string beskrivelse, bool gennemført)
        {
            var elevplan = await GetByElevIdAsync(elevId);
            
            if (elevplan != null)
            {
                var periode = elevplan.praktikperioder?.FirstOrDefault(p => p.periode_nummer == periodeNummer);
                var opgave = periode?.opgaver?.FirstOrDefault(o => o.kategori == kategori && o.beskrivelse == beskrivelse);
                
                if (opgave != null)
                {
                    opgave.gennemført = gennemført;
                    elevplan.opdateret_dato = DateTime.UtcNow;
                    
                    await _elevplanCollection.ReplaceOneAsync(
                        e => e.elev_id == elevId, 
                        elevplan
                    );
                }
            }
        }

        public async Task CreateAsync(Elevplan elevplan)
        {
            elevplan.oprettet_dato = DateTime.UtcNow;
            elevplan.opdateret_dato = DateTime.UtcNow;
            await _elevplanCollection.InsertOneAsync(elevplan);
        }
    }
}