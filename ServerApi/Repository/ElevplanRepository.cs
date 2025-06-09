using System;
using System.Collections.Generic;
using System.Linq;
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
            // Opretter forbindelse til MongoDB og henter samlingen 'Elevplan'
            var mongoUri = "mongodb+srv://benjaminlorenzen:pdx45bjd@" +
                           "cluster0.55cag.mongodb.net/Comwell?"  +
                           "retryWrites=true&w=majority";
            var client = new MongoClient(mongoUri);
            var db     = client.GetDatabase("Comwell");
            _elevplanCollection = db.GetCollection<Elevplan>("Elevplan");
        }

        // Henter alle elevplaner fra databasen
        public async Task<List<Elevplan>> GetAllAsync()
            => await _elevplanCollection.Find(_ => true).ToListAsync();

        // Returnerer en skabelon (template) til elevplanen (bruges til nye elever)
        public async Task<Elevplan?> GetTemplateAsync()
        {
            var filter = Builders<Elevplan>.Filter.Eq(p => p.ElevId, "TBD");
            return await _elevplanCollection.Find(filter).FirstOrDefaultAsync();
        }

        // Henter en elevplan ud fra elevens ID
        // STEP 6: Henter elevplan fra MongoDB -> svar sendes retur til ElevplanController
        public async Task<Elevplan?> GetByElevIdAsync(string elevId)
        {
            var filter = Builders<Elevplan>.Filter.Eq(p => p.ElevId, elevId);
            return await _elevplanCollection.Find(filter).FirstOrDefaultAsync();
        }

        // Opretter en ny elevplan og tildeler et unikt ID
        public async Task CreateAsync(Elevplan plan)
        {
            var maxIdDoc = await _elevplanCollection
                .Find(_ => true)
                .SortByDescending(p => p.Id)
                .Limit(1)
                .FirstOrDefaultAsync();

            plan.Id = (maxIdDoc?.Id ?? 0) + 1;
            await _elevplanCollection.InsertOneAsync(plan);
        }

        // Opdaterer status på en enkelt opgave (delmål) i en periode
        public async Task UpdateOpgaveAsync(string elevId, int periodeNummer, string kategori, string beskrivelse, bool gennemført)
        {
            var elevplan = await GetByElevIdAsync(elevId);
            if (elevplan == null) return;

            var periode = elevplan.Praktikperioder.Find(p => p.PeriodeNummer == periodeNummer);
            var opgave = periode?.Opgaver.Find(o => o.Kategori == kategori && o.Beskrivelse == beskrivelse);
            if (opgave == null) return;

            opgave.Gennemført = gennemført;
            elevplan.OpdateretDato = DateTime.UtcNow;

            await _elevplanCollection.ReplaceOneAsync(p => p.ElevId == elevId, elevplan);
        }

        // Tilføjer et nyt delmål til en bestemt periode
        public async Task<Delmaal> AddDelmaalAsync(string elevId, int periodeNummer, Delmaal delmaal)
        {
            var elevplan = await GetByElevIdAsync(elevId);
            if (elevplan == null)
                throw new KeyNotFoundException($"Elevplan med elevId '{elevId}' findes ikke.");

            var periode = elevplan.Praktikperioder
                .FirstOrDefault(p => p.PeriodeNummer == periodeNummer)
                ?? throw new KeyNotFoundException($"Periode {periodeNummer} findes ikke på denne elevplan.");

            periode.Opgaver.Add(delmaal);
            elevplan.OpdateretDato = DateTime.UtcNow;

            await _elevplanCollection.ReplaceOneAsync(p => p.ElevId == elevId, elevplan);

            return delmaal;
        }

        // Opdaterer eksisterende delmål i en periode
        public async Task<Delmaal?> UpdateDelmaalAsync(string elevId, int periodeNummer, Guid delmaalId, Delmaal updated)
        {
            var elevplan = await GetByElevIdAsync(elevId);
            if (elevplan == null) return null;

            var periode = elevplan.Praktikperioder.FirstOrDefault(p => p.PeriodeNummer == periodeNummer);
            if (periode == null) return null;

            var delmaal = periode.Opgaver.FirstOrDefault(d => d.DelmaalId == delmaalId);
            if (delmaal == null) return null;

            // Overskriver felter med nye værdier
            delmaal.Kategori    = updated.Kategori;
            delmaal.Beskrivelse = updated.Beskrivelse;
            delmaal.Ansvarlig   = updated.Ansvarlig;
            delmaal.Initiator   = updated.Initiator;
            delmaal.Tidslinje   = updated.Tidslinje;
            delmaal.Gennemført  = updated.Gennemført;

            elevplan.OpdateretDato = DateTime.UtcNow;

            await _elevplanCollection.ReplaceOneAsync(p => p.ElevId == elevId, elevplan);
            return delmaal;
        }

        // Sletter et delmål baseret på ID
        public async Task<bool> DeleteDelmaalAsync(string elevId, int periodeNummer, Guid delmaalId)
        {
            var elevplan = await GetByElevIdAsync(elevId);
            if (elevplan == null) return false;

            var periode = elevplan.Praktikperioder.FirstOrDefault(p => p.PeriodeNummer == periodeNummer);
            if (periode == null) return false;

            var removed = periode.Opgaver.RemoveAll(d => d.DelmaalId == delmaalId) > 0;
            if (!removed) return false;

            elevplan.OpdateretDato = DateTime.UtcNow;
            await _elevplanCollection.ReplaceOneAsync(p => p.ElevId == elevId, elevplan);

            return true;
        }

        // Returnerer liste af delmål i en bestemt periode
        public async Task<List<Delmaal>> GetDelmaalListeAsync(string elevId, int periodeNummer)
        {
            var elevplan = await GetByElevIdAsync(elevId);
            var periode = elevplan?.Praktikperioder.FirstOrDefault(p => p.PeriodeNummer == periodeNummer);

            return periode?.Opgaver.ToList() ?? new List<Delmaal>();
        }

        // Returnerer et specifikt delmål
        public async Task<Delmaal?> GetDelmaalAsync(string elevId, int periodeNummer, Guid delmaalId)
        {
            var liste = await GetDelmaalListeAsync(elevId, periodeNummer);
            return liste.FirstOrDefault(d => d.DelmaalId == delmaalId);
        }
    }
}
