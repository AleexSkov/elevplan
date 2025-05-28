using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Models
{
    public class Elevplan
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }

        [BsonElement("elev_id")]
        public string ElevId { get; set; } = default!;

        [BsonElement("elev_navn")]
        public string ElevNavn { get; set; } = default!;

        [BsonElement("aftaleform")]
        public string Aftaleform { get; set; } = default!;

        [BsonElement("skole")]
        public string Skole { get; set; } = default!;

        [BsonElement("praktikperioder")]
        public List<Praktikperiode> Praktikperioder { get; set; } = new();

        [BsonElement("oprettet_dato")]
        public DateTime OprettetDato { get; set; }

        [BsonElement("opdateret_dato")]
        public DateTime OpdateretDato { get; set; }
    }

    public class Praktikperiode
    {
        [BsonElement("periode_nummer")]
        public int PeriodeNummer { get; set; }

        [BsonElement("varighed_uger")]
        public int VarighedUger { get; set; }

        [BsonElement("skoleperiode")]
        public Skoleperiode Skoleperiode { get; set; } = default!;

        [BsonElement("opgaver")]
        public List<Delmaal> Opgaver { get; set; } = new();
    }

    public class Skoleperiode
    {
        [BsonElement("start_dato")]
        public DateTime? StartDato { get; set; }

        [BsonElement("slut_dato")]
        public DateTime? SlutDato { get; set; }

        [BsonElement("varighed_uger")]
        public int VarighedUger { get; set; }
    }
}