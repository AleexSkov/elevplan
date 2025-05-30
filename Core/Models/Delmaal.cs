using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Models
{
    /// <summary>
    /// Repræsenterer et enkelt delmål i en praktikperiode for en elev.
    /// Indeholder metadata som kategori, ansvarlig og status.
    /// </summary>
    public class Delmaal
    {
        /// <summary>
        /// Unikt ID for delmålet (bruges som identifikator i systemet).
        /// Gemmes i MongoDB som en streng under navnet "delmaal_id".
        /// </summary>
        [BsonElement("delmaal_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid DelmaalId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Overordnet kategori som delmålet tilhører (f.eks. "Madlavning").
        /// </summary>
        [BsonElement("kategori")]
        public string Kategori { get; set; } = default!;

        /// <summary>
        /// Detaljeret beskrivelse af delmålet.
        /// </summary>
        [BsonElement("beskrivelse")]
        public string Beskrivelse { get; set; } = default!;

        /// <summary>
        /// Den person, der er ansvarlig for delmålet (f.eks. kok, mentor).
        /// </summary>
        [BsonElement("ansvarlig")]
        public string Ansvarlig { get; set; } = default!;

        /// <summary>
        /// Den person, der har taget initiativ til delmålet (f.eks. elev, HR).
        /// </summary>
        [BsonElement("initiator")]
        public string Initiator { get; set; } = default!;

        /// <summary>
        /// Tidslinje eller deadline for hvornår delmålet skal være opfyldt.
        /// </summary>
        [BsonElement("tidslinje")]
        public string Tidslinje { get; set; } = default!;

        /// <summary>
        /// Om delmålet er gennemført eller ej (afkrydsningsstatus).
        /// </summary>
        [BsonElement("gennemført")]
        public bool Gennemført { get; set; }
    }
}