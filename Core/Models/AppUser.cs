using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Models
{
    public class AppUser
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool MustChangePassword { get; set; } = true;
        public string? Name { get; set; }
    }
}