using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interface
{
    public interface IElevplan
    {
        Task<List<Elevplan>> GetAllAsync();
        Task<Elevplan?> GetByElevIdAsync(string elevId);
        Task UpdateOpgaveAsync(string elevId, int periodeNummer, string kategori, string beskrivelse, bool gennemført);
        Task CreateAsync(Elevplan elevplan);
    }
}