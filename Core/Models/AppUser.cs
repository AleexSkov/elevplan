using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Models
{
    /// <summary>
    /// Repræsenterer en bruger i systemet – f.eks. elev eller admin.
    /// Gemmes i MongoDB.
    /// </summary>
    public class AppUser
    {
        [BsonId]
        public ObjectId Id { get; set; }                        // Unik MongoDB-identifikator

        public string Email         { get; set; } = null!;      // Brugerens e-mail (og login-navn)
        public string Role          { get; set; } = null!;      // Rolle i systemet, fx "Elev" eller "Admin"
        public string PasswordHash  { get; set; } = null!;      // Hash'et adgangskode

        public DateTime CreatedAt   { get; set; } = DateTime.UtcNow;  // Tidspunkt for oprettelse (UTC)

        public bool MustChangePassword { get; set; } = true;    // Om brugeren skal ændre kode ved første login

        public string? Name         { get; set; }               // Navn (kan være null ved registrering)
    }
}