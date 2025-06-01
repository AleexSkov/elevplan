using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Models
{
    /// <summary>
    /// Elevens samlede plan – indeholder oplysninger og praktikperioder.
    /// </summary>
    public class Elevplan
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; } // Intern numerisk ID (MongoDB understøtter flere typer ID)

        [BsonElement("elev_id")]
        public string ElevId { get; set; } = default!; // Unik identifikator for eleven

        [BsonElement("elev_navn")]
        public string ElevNavn { get; set; } = default!; // Navn på eleven

        [BsonElement("aftaleform")]
        public string Aftaleform { get; set; } = default!; // Fx. "EUX", "Grundforløb", "Hovedforløb"

        [BsonElement("skole")]
        public string Skole { get; set; } = default!; // Fx. "Hotel- og Restaurantskolen"

        [BsonElement("praktikperioder")]
        public List<Praktikperiode> Praktikperioder { get; set; } = new(); // Liste over praktikperioder

        [BsonElement("oprettet_dato")]
        public DateTime OprettetDato { get; set; } // Hvornår elevplanen blev oprettet

        [BsonElement("opdateret_dato")]
        public DateTime OpdateretDato { get; set; } // Hvornår den sidst blev redigeret
    }

    /// <summary>
    /// En praktikperiode som del af en elevplan – indeholder skoleperiode og opgaver.
    /// </summary>
    public class Praktikperiode
    {
        [BsonElement("periode_nummer")]
        public int PeriodeNummer { get; set; } // Fx. 1, 2, 3…

        [BsonElement("varighed_uger")]
        public int VarighedUger { get; set; } // Hvor lang perioden varer

        [BsonElement("skoleperiode")]
        public Skoleperiode Skoleperiode { get; set; } = default!; // Datoer for evt. skoleforløb

        [BsonElement("opgaver")]
        public List<Delmaal> Opgaver { get; set; } = new(); // Liste over tilhørende delmål (opgaver)
    }

    /// <summary>
    /// Valgfri information om skoleforløb i en praktikperiode.
    /// </summary>
    public class Skoleperiode
    {
        [BsonElement("start_dato")]
        public DateTime? StartDato { get; set; } // Startdato for skoleperioden

        [BsonElement("slut_dato")]
        public DateTime? SlutDato { get; set; } // Slutdato for skoleperioden

        [BsonElement("varighed_uger")]
        public int VarighedUger { get; set; } // Hvor lang skoleperioden er
    }
}
