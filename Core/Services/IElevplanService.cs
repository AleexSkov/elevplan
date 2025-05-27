using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Services
{
    public interface IElevplanService
    {
        Task<List<Elevplan>> GetAllElevplanerAsync();
        Task<List<ElevInfo>> GetAlleEleverAsync();
        Task<ElevplanViewModel> GetElevplanViewModelAsync(string elevId);
        Task UpdateOpgaveStatusAsync(string elevId, int periodeNummer, string kategori, string beskrivelse, bool gennemført);
        Task<List<ProgressSummary>> GetProgressSummaryAsync(string elevId);
    }

    // ✅ ElevInfo med ALLE nødvendige properties
    public class ElevInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Navn { get; set; } = string.Empty; 
        public string Skole { get; set; } = string.Empty;
        public string Aftaleform { get; set; } = string.Empty;
        public DateTime OprettetDato { get; set; }
        public DateTime OpdateretDato { get; set; }
    }

    public class ElevplanViewModel
    {
        public ElevInfo Elev { get; set; } = new();
        public List<PeriodViewModel> Perioder { get; set; } = new();
    }

    public class PeriodViewModel
    {
        public int PeriodeNummer { get; set; }
        public string Title => $"Periode {PeriodeNummer}";
        public string Duration => $"{VarighedUger} uger";
        public string SchoolPeriod => $"{SkoleperiodeUger} uger";
        public int VarighedUger { get; set; }
        public int SkoleperiodeUger { get; set; }
        public List<CategoryViewModel> Categories { get; set; } = new();
    }

    public class CategoryViewModel
    {
        public string Name { get; set; } = string.Empty;
        public List<TaskViewModel> Tasks { get; set; } = new();
    }

    public class TaskViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Responsible { get; set; } = string.Empty;
        public string Deadline { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public string Kategori { get; set; } = string.Empty;
    }

    public class ProgressSummary
    {
        public string PeriodTitle { get; set; } = string.Empty;
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public double ProgressPercentage => TotalTasks > 0 ? (double)CompletedTasks / TotalTasks * 100 : 0;
    }
}