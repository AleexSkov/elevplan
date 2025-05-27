using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Core.Models;
using Core.Interface;

namespace Core.Services
{
    public class ElevplanService : IElevplanService
    {
        private readonly IElevplan _elevplanRepo;

        public ElevplanService(IElevplan elevplanRepo)
        {
            _elevplanRepo = elevplanRepo;
        }

        public async Task<List<Elevplan>> GetAllElevplanerAsync()
        {
            return await _elevplanRepo.GetAllAsync();
        }

        public async Task<List<ElevInfo>> GetAlleEleverAsync()
        {
            var elevplaner = await _elevplanRepo.GetAllAsync();
            return elevplaner.Select(e => new ElevInfo
            {
                Id = e.elev_id,
                Navn = e.elev_navn,
                Skole = e.skole,
                Aftaleform = e.aftaleform,
                OprettetDato = e.oprettet_dato,  
                OpdateretDato = e.opdateret_dato 
            }).ToList();
        }

        public async Task<ElevplanViewModel> GetElevplanViewModelAsync(string elevId)
        {
            var elevplaner = await _elevplanRepo.GetAllAsync();
            var elevplan = elevplaner.FirstOrDefault(e => e.elev_id == elevId);
            
            if (elevplan == null)
            {
                return new ElevplanViewModel();
            }

            return new ElevplanViewModel
            {
                Elev = new ElevInfo 
                {
                    Id = elevplan.elev_id,
                    Navn = elevplan.elev_navn,
                    Skole = elevplan.skole,
                    Aftaleform = elevplan.aftaleform,
                    OprettetDato = elevplan.oprettet_dato,  
                    OpdateretDato = elevplan.opdateret_dato 
                },
                Perioder = MapPerioder(elevplan.praktikperioder ?? new List<Praktikperiode>())
            };
        }

        public async Task UpdateOpgaveStatusAsync(string elevId, int periodeNummer, string kategori, string beskrivelse, bool gennemført)
        {
            await _elevplanRepo.UpdateOpgaveAsync(elevId, periodeNummer, kategori, beskrivelse, gennemført);
        }

        public async Task<List<ProgressSummary>> GetProgressSummaryAsync(string elevId)
        {
            var elevplan = await GetElevplanViewModelAsync(elevId);
            
            return elevplan.Perioder.Select(p => new ProgressSummary
            {
                PeriodTitle = p.Title,
                TotalTasks = p.Categories.Sum(c => c.Tasks.Count),
                CompletedTasks = p.Categories.Sum(c => c.Tasks.Count(t => t.IsCompleted))
            }).ToList();
        }

        private List<PeriodViewModel> MapPerioder(List<Praktikperiode> praktikperioder)
        {
            return praktikperioder.Select(p => new PeriodViewModel
            {
                PeriodeNummer = p.periode_nummer,
                VarighedUger = p.varighed_uger,
                SkoleperiodeUger = p.skoleperiode?.varighed_uger ?? 0,
                Categories = MapKategorier(p.opgaver ?? new List<Opgave>())
            }).ToList();
        }

        private List<CategoryViewModel> MapKategorier(List<Opgave> opgaver)
        {
            return opgaver
                .GroupBy(o => o.kategori)
                .Select(g => new CategoryViewModel
                {
                    Name = g.Key,
                    Tasks = g.Select(o => new TaskViewModel
                    {
                        Title = o.beskrivelse,
                        Responsible = o.ansvarlig,
                        Deadline = o.tidslinje,
                        IsCompleted = o.gennemført,
                        Kategori = o.kategori
                    }).ToList()
                }).ToList();
        }
    }
}