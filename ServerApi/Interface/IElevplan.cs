using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace ServerApi.Interface
{
    /// <summary>
    /// Interface til håndtering af Elevplan-data og relaterede CRUD-operationer.
    /// Abstraherer datatilgang til MongoDB (eller anden lagring).
    /// </summary>
    public interface IElevplan
    {
        // ----------------------
        // ELEVPLAN OVERORDNET
        // ----------------------

        /// <summary>
        /// Henter alle elevplaner i systemet (fx til admin-visning).
        /// </summary>
        Task<List<Elevplan>> GetAllAsync();

        /// <summary>
        /// Henter en skabelon/elevplan-template til nye elever.
        /// Returneres typisk ved oprettelse af ny plan.
        /// </summary>
        Task<Elevplan?> GetTemplateAsync();

        /// <summary>
        /// Henter en specifik elevs plan via elevens ID.
        /// Bruges ved indlæsning af plan i UI.
        /// </summary>
        Task<Elevplan?> GetByElevIdAsync(string elevId);

        /// <summary>
        /// Opretter en ny elevplan i databasen.
        /// Bruges fx ved første login/oprettelse.
        /// </summary>
        Task CreateAsync(Elevplan plan);

        /// <summary>
        /// Opdaterer gennemført-status for en opgave i en bestemt periode.
        /// Bruges til checkbox-funktionalitet.
        /// </summary>
        Task UpdateOpgaveAsync(
            string elevId,
            int    periodeNummer,
            string kategori,
            string beskrivelse,
            bool   gennemført);

        // ----------------------
        // CRUD FOR DELMÅL
        // ----------------------

        /// <summary>
        /// Tilføjer et nyt delmål til en bestemt periode for en elev.
        /// </summary>
        Task<Delmaal> AddDelmaalAsync(string elevId, int periodeNummer, Delmaal delmaal);

        /// <summary>
        /// Opdaterer et eksisterende delmål baseret på ID.
        /// Returnerer null hvis det ikke blev fundet.
        /// </summary>
        Task<Delmaal?> UpdateDelmaalAsync(string elevId, int periodeNummer, Guid delmaalId, Delmaal updated);

        /// <summary>
        /// Sletter et delmål for en elev i en given periode.
        /// Returnerer true/false afhængigt af om det lykkedes.
        /// </summary>
        Task<bool> DeleteDelmaalAsync(string elevId, int periodeNummer, Guid delmaalId);

        /// <summary>
        /// Henter listen over delmål for en given elev og periode.
        /// Bruges ved visning af periodeopgaver.
        /// </summary>
        Task<List<Delmaal>> GetDelmaalListeAsync(string elevId, int periodeNummer);

        /// <summary>
        /// Henter et enkelt delmål baseret på ID.
        /// Bruges fx til detaljeret visning/redigering.
        /// </summary>
        Task<Delmaal?> GetDelmaalAsync(string elevId, int periodeNummer, Guid delmaalId);
    }
}
