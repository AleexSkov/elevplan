using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Elevplan
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("elev_id")]
        public string ElevId { get; set; }

        [BsonElement("elev_navn")]
        public string ElevNavn { get; set; }

        [BsonElement("aftaleform")]
        public string Aftaleform { get; set; }

        [BsonElement("skole")]
        public string Skole { get; set; }

        [BsonElement("praktikperioder")]
        public List<Praktikperiode> Praktikperioder { get; set; }

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
        public Skoleperiode Skoleperiode { get; set; }

        [BsonElement("opgaver")]
        public List<Opgave> Opgaver { get; set; }
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

    public class Opgave
    {
        [BsonElement("kategori")]
        public string Kategori { get; set; }

        [BsonElement("beskrivelse")]
        public string Beskrivelse { get; set; }

        [BsonElement("ansvarlig")]
        public string Ansvarlig { get; set; }

        [BsonElement("initiator")]
        public string Initiator { get; set; }

        [BsonElement("tidslinje")]
        public string Tidslinje { get; set; }

        [BsonElement("gennemført")]
        public bool Gennemført { get; set; }
    }
}