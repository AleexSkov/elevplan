using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Elevplan
    {
        public int _id { get; set; }
        public string elev_id { get; set; }
        public string elev_navn { get; set; }
        public string aftaleform { get; set; }
        public string skole { get; set; }
        public List<Praktikperiode> praktikperioder { get; set; }
        public DateTime oprettet_dato { get; set; }
        public DateTime opdateret_dato { get; set; }
    }

    public class Praktikperiode
    {
        public int periode_nummer { get; set; }
        public int varighed_uger { get; set; }
        public Skoleperiode skoleperiode { get; set; }
        public List<Opgave> opgaver { get; set; }
    }

    public class Skoleperiode
    {
        public DateTime? start_dato { get; set; }
        public DateTime? slut_dato { get; set; }
        public int varighed_uger { get; set; }
    }

    public class Opgave
    {
        public string kategori { get; set; }
        public string beskrivelse { get; set; }
        public string ansvarlig { get; set; }
        public string initiator { get; set; }
        public string tidslinje { get; set; }
        public bool gennemført { get; set; }
    }
}