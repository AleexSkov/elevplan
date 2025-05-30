using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using MongoDB.Bson;

namespace ServerApi.Interface
{
    /// <summary>
    /// Interface for datatilgang og håndtering af AppUser-brugere (MongoDB).
    /// Bruges til dependency injection og løsrivelse af implementering.
    /// </summary>
    public interface IAppUser
    {
        /// <summary>
        /// Henter en liste over alle brugere i databasen.
        /// </summary>
        Task<List<AppUser>> GetAllAsync();

        /// <summary>
        /// Henter en bruger baseret på deres ObjectId.
        /// Returnerer null hvis brugeren ikke findes.
        /// </summary>
        Task<AppUser?> GetByIdAsync(ObjectId id);

        /// <summary>
        /// Henter en bruger baseret på deres e-mailadresse.
        /// Bruges især til login og validering.
        /// </summary>
        Task<AppUser?> GetByEmailAsync(string email);

        /// <summary>
        /// Opretter en ny bruger i databasen.
        /// </summary>
        Task CreateAsync(AppUser user);

        /// <summary>
        /// Tjekker om en given e-mail allerede eksisterer i systemet.
        /// Bruges til validering ved oprettelse.
        /// </summary>
        Task<bool> EmailExistsAsync(string email);

        /// <summary>
        /// Opdaterer en eksisterende bruger baseret på ObjectId.
        /// Bruges til fx at ændre adgangskode, rolle eller navn.
        /// </summary>
        Task UpdateAsync(ObjectId id, AppUser updatedUser);
    }
}