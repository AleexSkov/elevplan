using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace ServerApi.Interface
{
    public interface IElevplan
    {
        // Eksisterende Elevplan-metoder
        Task<List<Elevplan>> GetAllAsync();
        Task<Elevplan?> GetTemplateAsync();
        Task<Elevplan?> GetByElevIdAsync(string elevId);
        Task CreateAsync(Elevplan plan);
        Task UpdateOpgaveAsync(
            string elevId,
            int    periodeNummer,
            string kategori,
            string beskrivelse,
            bool   gennemført);

        // CRUD for delmål (opgaver)
        Task<Delmaal> AddDelmaalAsync(string elevId, int periodeNummer, Delmaal delmaal);
        Task<Delmaal?> UpdateDelmaalAsync(string elevId, int periodeNummer, Guid delmaalId, Delmaal updated);
        Task<bool> DeleteDelmaalAsync(string elevId, int periodeNummer, Guid delmaalId);
        Task<List<Delmaal>> GetDelmaalListeAsync(string elevId, int periodeNummer);
        Task<Delmaal?> GetDelmaalAsync(string elevId, int periodeNummer, Guid delmaalId);
    }
}