using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Core.Models;
using ServerApi.Interface;

namespace ServerApi.Repository
{
    public partial class ElevplanRepository : IElevplan
    {
        private readonly IMongoCollection<Elevplan> _elevplanCollection;

        public ElevplanRepository(IMongoCollection<Elevplan> elevplanCollection)
        {
            _elevplanCollection = elevplanCollection;
        }

        public async Task<List<Elevplan>> GetAllAsync()
            => await _elevplanCollection.Find(_ => true).ToListAsync();

        public async Task<Elevplan?> GetTemplateAsync()
        {
            var filter = Builders<Elevplan>.Filter.Eq(p => p.ElevId, "TBD");
            return await _elevplanCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Elevplan?> GetByElevIdAsync(string elevId)
        {
            var filter = Builders<Elevplan>.Filter.Eq(p => p.ElevId, elevId);
            return await _elevplanCollection.Find(filter).FirstOrDefaultAsync();
        }

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

        public async Task UpdateOpgaveAsync(
            string elevId,
            int periodeNummer,
            string kategori,
            string beskrivelse,
            bool gennemført)
        {
            var elevplan = await GetByElevIdAsync(elevId);
            if (elevplan == null) return;

            var periode = elevplan.Praktikperioder
                .Find(p => p.PeriodeNummer == periodeNummer);
            var opgave = periode?.Opgaver
                .Find(o => o.Kategori == kategori && o.Beskrivelse == beskrivelse);
            if (opgave == null) return;

            opgave.Gennemført   = gennemført;
            elevplan.OpdateretDato = System.DateTime.UtcNow;

            await _elevplanCollection.ReplaceOneAsync(
                p => p.ElevId == elevId, elevplan);
        }
    }
}
