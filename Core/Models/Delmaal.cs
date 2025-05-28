using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Models
{
    public class Delmaal
    {
        // Unikt ID for hvert delmål (gemmes som delmaal_id i MongoDB)
        [BsonElement("delmaal_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid DelmaalId { get; set; } = Guid.NewGuid();

        [BsonElement("kategori")]
        public string Kategori    { get; set; } = default!;

        [BsonElement("beskrivelse")]
        public string Beskrivelse { get; set; } = default!;

        [BsonElement("ansvarlig")]
        public string Ansvarlig   { get; set; } = default!;

        [BsonElement("initiator")]
        public string Initiator   { get; set; } = default!;

        [BsonElement("tidslinje")]
        public string Tidslinje   { get; set; } = default!;

        [BsonElement("gennemført")]
        public bool   Gennemført   { get; set; }
    }
}