using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Models
{
    public class Elevplan
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string elev_id { get; set; } = string.Empty;
        public string elev_navn { get; set; } = string.Empty;
        public string aftaleform { get; set; } = string.Empty;
        public string skole { get; set; } = string.Empty;
        public List<Praktikperiode> praktikperioder { get; set; } = new(); 
        public DateTime oprettet_dato { get; set; }
        public DateTime opdateret_dato { get; set; }
    }

    public class Praktikperiode
    {
        public int periode_nummer { get; set; }
        public int varighed_uger { get; set; }
        public Skoleperiode? skoleperiode { get; set; }
        public List<Opgave> opgaver { get; set; } = new();
    }

    public class Skoleperiode
    {
        public DateTime? start_dato { get; set; }
        public DateTime? slut_dato { get; set; }
        public int varighed_uger { get; set; }
    }

    public class Opgave
    {
        public string kategori { get; set; } = string.Empty;
        public string beskrivelse { get; set; } = string.Empty;
        public string ansvarlig { get; set; } = string.Empty;
        public string initiator { get; set; } = string.Empty;
        public string tidslinje { get; set; } = string.Empty;
        public bool gennemført { get; set; }
    }
}