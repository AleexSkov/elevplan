using Core.Interface;
using Core.Models;

namespace ComwellElevplan.Data
{
    /// <summary>
    /// Mock repository med realistisk Comwell elevplan data
    /// Kan erstattes med rigtig MongoDB repository senere
    /// </summary>
    public class MockElevplanRepository : IElevplan
    {
        public async Task<List<Elevplan>> GetAllAsync()
        {
            await Task.Delay(100);
            return GetMockElevplaner();
        }

        public async Task<Elevplan?> GetByElevIdAsync(string elevId)
        {
            var all = await GetAllAsync();
            return all.FirstOrDefault(e => e.elev_id == elevId);
        }

        public async Task UpdateOpgaveAsync(string elevId, int periodeNummer, string kategori, string beskrivelse, bool gennemført)
        {
            await Task.Delay(50);
            Console.WriteLine($"Updated: {beskrivelse} = {gennemført} for elev {elevId}");
            // I en rigtig implementation ville vi opdatere databasen her
        }

        public async Task CreateAsync(Elevplan elevplan)
        {
            await Task.Delay(50);
            Console.WriteLine($"Created elevplan for: {elevplan.elev_navn}");
            // I en rigtig implementation ville vi gemme i databasen her
        }

        /// <summary>
        /// Genererer 3 realistiske elever med faktiske Comwell opgaver
        /// </summary>
        private static List<Elevplan> GetMockElevplaner()
        {
            return new List<Elevplan>
            {
                CreateAnnaAndersen(),
                CreateMadsNielsen(), 
                CreateSofiaLarsen()
            };
        }

        /// <summary>
        /// Anna Andersen - Periode 1 (nystartet elev)
        /// </summary>
        private static Elevplan CreateAnnaAndersen()
        {
            return new Elevplan
            {
                elev_id = "1",
                elev_navn = "Anna Andersen",
                skole = "Hotel- og Restaurantskolen København",
                aftaleform = "ordinær",
                oprettet_dato = DateTime.Now.AddDays(-30),
                opdateret_dato = DateTime.Now,
                praktikperioder = new List<Praktikperiode>
                {
                    new Praktikperiode
                    {
                        periode_nummer = 1,
                        varighed_uger = 52,
                        skoleperiode = new Skoleperiode 
                        { 
                            varighed_uger = 10,
                            start_dato = DateTime.Now.AddDays(60),
                            slut_dato = DateTime.Now.AddDays(130)
                        },
                        opgaver = new List<Opgave>
                        {
                            // Inden første dag - de fleste gennemført
                            new Opgave 
                            { 
                                kategori = "Inden første dag", 
                                beskrivelse = "Bestil uniform", 
                                ansvarlig = "Elevansvarlig", 
                                initiator = "Elevansvarlig / Nærmeste leder",
                                tidslinje = "Inden første arbejdsdag", 
                                gennemført = true 
                            },
                            new Opgave 
                            { 
                                kategori = "Inden første dag", 
                                beskrivelse = "Informer om forplejning", 
                                ansvarlig = "Elevansvarlig / Nærmeste leder", 
                                initiator = "Elevansvarlig / Nærmeste leder",
                                tidslinje = "Inden første arbejdsdag", 
                                gennemført = true 
                            },
                            new Opgave 
                            { 
                                kategori = "Inden første dag", 
                                beskrivelse = "Adgang til Comwell Connect", 
                                ansvarlig = "Elevansvarlig / Nærmeste leder", 
                                initiator = "Elevansvarlig / Nærmeste leder",
                                tidslinje = "Inden første arbejdsdag", 
                                gennemført = true 
                            },
                            new Opgave 
                            { 
                                kategori = "Inden første dag", 
                                beskrivelse = "Bestil evt. adgangskort/nøgle", 
                                ansvarlig = "AV/Ejendom", 
                                initiator = "Elevansvarlig / Nærmeste leder",
                                tidslinje = "Inden første arbejdsdag", 
                                gennemført = false 
                            },
                            new Opgave 
                            { 
                                kategori = "Inden første dag", 
                                beskrivelse = "Evt. opkald fra elevkollega - Vi glæder os til du starter", 
                                ansvarlig = "Elevkollega/elevmentor", 
                                initiator = "Elevansvarlig / Nærmeste leder",
                                tidslinje = "Inden første arbejdsdag", 
                                gennemført = true 
                            },

                            // Velkommen og introduktion
                            new Opgave 
                            { 
                                kategori = "Velkommen til og introduktion til kollegaer", 
                                beskrivelse = "Udlever tøj og sikkerhedssko", 
                                ansvarlig = "Elevansvarlig / Nærmeste leder", 
                                initiator = "Nærmeste leder",
                                tidslinje = "Den første uge", 
                                gennemført = true 
                            },
                            new Opgave 
                            { 
                                kategori = "Velkommen til og introduktion til kollegaer", 
                                beskrivelse = "Hvor er omklædning, personalekantine, toiletter?", 
                                ansvarlig = "Elevansvarlig / Nærmeste leder", 
                                initiator = "Nærmeste leder",
                                tidslinje = "Den første uge", 
                                gennemført = true 
                            },
                            new Opgave 
                            { 
                                kategori = "Velkommen til og introduktion til kollegaer", 
                                beskrivelse = "Hvor er mit skab til personlige ejendele og skuffe til egne knive?", 
                                ansvarlig = "Elevansvarlig / Nærmeste leder", 
                                initiator = "Nærmeste leder",
                                tidslinje = "Den første uge", 
                                gennemført = true 
                            },
                            new Opgave 
                            { 
                                kategori = "Velkommen til og introduktion til kollegaer", 
                                beskrivelse = "Hvor stempler jeg ind og ud - og hvordan?", 
                                ansvarlig = "Elevansvarlig / Nærmeste leder", 
                                initiator = "Nærmeste leder",
                                tidslinje = "Den første uge", 
                                gennemført = false 
                            },
                            new Opgave 
                            { 
                                kategori = "Velkommen til og introduktion til kollegaer", 
                                beskrivelse = "Rundvisning på hotellet", 
                                ansvarlig = "Elevansvarlig / Nærmeste leder", 
                                initiator = "Nærmeste leder",
                                tidslinje = "Den første uge", 
                                gennemført = false 
                            },

                            // Sikkerhed og arbejdsmiljø
                            new Opgave 
                            { 
                                kategori = "Sikkerhed og arbejdsmiljø", 
                                beskrivelse = "Introduktion til arbejdsmiljø på Comwell Connect og AMU på hotellet", 
                                ansvarlig = "Elevansvarlig", 
                                initiator = "Nærmeste leder",
                                tidslinje = "I løbet af den første måned", 
                                gennemført = false 
                            },
                            new Opgave 
                            { 
                                kategori = "Sikkerhed og arbejdsmiljø", 
                                beskrivelse = "Ergonomi - herunder tunge løft", 
                                ansvarlig = "Elevansvarlig", 
                                initiator = "Nærmeste leder",
                                tidslinje = "I løbet af den første måned", 
                                gennemført = false 
                            },
                            new Opgave 
                            { 
                                kategori = "Sikkerhed og arbejdsmiljø", 
                                beskrivelse = "Sikkerhedsrutiner i et køkken", 
                                ansvarlig = "Elevansvarlig", 
                                initiator = "Nærmeste leder",
                                tidslinje = "I løbet af den første måned", 
                                gennemført = false 
                            },

                            // Faglige mål
                            new Opgave 
                            { 
                                kategori = "Faglige mål", 
                                beskrivelse = "Kendskab og gennemgang af systemer: iVvy (Selskabsbookingsystem)", 
                                ansvarlig = "Nærmeste leder/anden", 
                                initiator = "Elevansvarlig / Nærmeste leder",
                                tidslinje = "I praktikperioden", 
                                gennemført = false 
                            },
                            new Opgave 
                            { 
                                kategori = "Faglige mål", 
                                beskrivelse = "Gennemgang af de forskellige knives funktionalitet og brugsegenskaber", 
                                ansvarlig = "Nærmeste leder/anden", 
                                initiator = "Elevansvarlig / Nærmeste leder",
                                tidslinje = "Efter prøvetid", 
                                gennemført = false 
                            },
                            new Opgave 
                            { 
                                kategori = "Faglige mål", 
                                beskrivelse = "Kvalitetskendetegn på råvarerne: Kød", 
                                ansvarlig = "Nærmeste leder/anden", 
                                initiator = "Elevansvarlig / Nærmeste leder",
                                tidslinje = "I praktikperioden", 
                                gennemført = false 
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Mads Nielsen - Periode 2 (erfaren elev)
        /// </summary>
        private static Elevplan CreateMadsNielsen()
        {
            return new Elevplan
            {
                elev_id = "2",
                elev_navn = "Mads Nielsen",
                skole = "EUC Sjælland",
                aftaleform = "ordinær",
                oprettet_dato = DateTime.Now.AddDays(-180),
                opdateret_dato = DateTime.Now.AddDays(-2),
                praktikperioder = new List<Praktikperiode>
                {
                    new Praktikperiode
                    {
                        periode_nummer = 1,
                        varighed_uger = 52,
                        skoleperiode = new Skoleperiode { varighed_uger = 10 },
                        opgaver = new List<Opgave>
                        {
                            new Opgave { kategori = "Afsluttet", beskrivelse = "Alle grundlæggende opgaver gennemført", ansvarlig = "Elevansvarlig", initiator = "Elevansvarlig", tidslinje = "Periode 1", gennemført = true }
                        }
                    },
                    new Praktikperiode
                    {
                        periode_nummer = 2,
                        varighed_uger = 43,
                        skoleperiode = new Skoleperiode 
                        { 
                            varighed_uger = 10,
                            start_dato = DateTime.Now.AddDays(-90),
                            slut_dato = DateTime.Now.AddDays(-20)
                        },
                        opgaver = new List<Opgave>
                        {
                            // Evaluering
                            new Opgave { kategori = "Evaluering", beskrivelse = "Første praktikperiode - nåede du gennem alle målene?", ansvarlig = "Elevansvarlig / Nærmeste leder", initiator = "Nærmeste leder", tidslinje = "Efter skoleperiode 1", gennemført = true },
                            new Opgave { kategori = "Evaluering", beskrivelse = "Første skoleperiode og pensum - var der noget, du manglede?", ansvarlig = "Elevansvarlig / Nærmeste leder", initiator = "Nærmeste leder", tidslinje = "Efter skoleperiode 1", gennemført = true },
                            new Opgave { kategori = "Evaluering", beskrivelse = "Gennemgang af uddannelsesplanen for kommende praktikperiode", ansvarlig = "Elevansvarlig / Nærmeste leder", initiator = "Nærmeste leder", tidslinje = "Efter skoleperiode 1", gennemført = true },

                            // Faglige mål periode 2
                            new Opgave { kategori = "Faglige mål", beskrivelse = "Udskæring af kød", ansvarlig = "Nærmeste leder/anden", initiator = "Elevansvarlig / Nærmeste leder", tidslinje = "I praktikperiode 2", gennemført = true },
                            new Opgave { kategori = "Faglige mål", beskrivelse = "Fisk, skaldyr og bløddyr, fjerkræ", ansvarlig = "Nærmeste leder/anden", initiator = "Elevansvarlig / Nærmeste leder", tidslinje = "I praktikperiode 2", gennemført = true },
                            new Opgave { kategori = "Faglige mål", beskrivelse = "Selvstændigt kunne lave varieret personalemad", ansvarlig = "Nærmeste leder/anden", initiator = "Elevansvarlig / Nærmeste leder", tidslinje = "I praktikperiode 2", gennemført = false },
                            new Opgave { kategori = "Faglige mål", beskrivelse = "Selvstændigt kunne køre banket/selskaber (30+ kuverter)", ansvarlig = "Nærmeste leder/anden", initiator = "Elevansvarlig / Nærmeste leder", tidslinje = "I praktikperiode 2", gennemført = false },
                            new Opgave { kategori = "Faglige mål", beskrivelse = "Økologi: Definér bronze, sølv og guld økologimærkning", ansvarlig = "Nærmeste leder/anden", initiator = "Elevansvarlig / Nærmeste leder", tidslinje = "I praktikperiode 2", gennemført = false }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Sofia Larsen - Periode 3 (næsten færdig)
        /// </summary>
        private static Elevplan CreateSofiaLarsen()
        {
            return new Elevplan
            {
                elev_id = "3",
                elev_navn = "Sofia Larsen",
                skole = "SOSU Østjylland",
                aftaleform = "ordinær",
                oprettet_dato = DateTime.Now.AddDays(-300),
                opdateret_dato = DateTime.Now.AddDays(-1),
                praktikperioder = new List<Praktikperiode>
                {
                    new Praktikperiode
                    {
                        periode_nummer = 1,
                        varighed_uger = 52,
                        skoleperiode = new Skoleperiode { varighed_uger = 10 },
                        opgaver = new List<Opgave>
                        {
                            new Opgave { kategori = "Afsluttet", beskrivelse = "Alle grundlæggende opgaver gennemført", ansvarlig = "Elevansvarlig", initiator = "Elevansvarlig", tidslinje = "Periode 1", gennemført = true }
                        }
                    },
                    new Praktikperiode
                    {
                        periode_nummer = 2,
                        varighed_uger = 43,
                        skoleperiode = new Skoleperiode { varighed_uger = 10 },
                        opgaver = new List<Opgave>
                        {
                            new Opgave { kategori = "Afsluttet", beskrivelse = "Alle avancerede opgaver gennemført", ansvarlig = "Elevansvarlig", initiator = "Elevansvarlig", tidslinje = "Periode 2", gennemført = true }
                        }
                    },
                    new Praktikperiode
                    {
                        periode_nummer = 3,
                        varighed_uger = 43,
                        skoleperiode = new Skoleperiode 
                        { 
                            varighed_uger = 7,
                            start_dato = DateTime.Now.AddDays(-50),
                            slut_dato = DateTime.Now.AddDays(-1)
                        },
                        opgaver = new List<Opgave>
                        {
                            // Evaluering
                            new Opgave { kategori = "Evaluering", beskrivelse = "Anden praktikperiode - nåede du gennem alle målene?", ansvarlig = "Elevansvarlig / Nærmeste leder", initiator = "Nærmeste leder", tidslinje = "Efter skoleperiode 2", gennemført = true },
                            new Opgave { kategori = "Evaluering", beskrivelse = "Anden skoleperiode og pensum - var der noget, du manglede?", ansvarlig = "Elevansvarlig / Nærmeste leder", initiator = "Nærmeste leder", tidslinje = "Efter skoleperiode 2", gennemført = true },
                            new Opgave { kategori = "Evaluering", beskrivelse = "Gennemgang af uddannelsesplanen for kommende praktikperiode", ansvarlig = "Elevansvarlig / Nærmeste leder", initiator = "Nærmeste leder", tidslinje = "Efter skoleperiode 2", gennemført = true },

                            // Faglige mål periode 3
                            new Opgave { kategori = "Faglige mål", beskrivelse = "Menusammensætning", ansvarlig = "Nærmeste leder/anden", initiator = "Elevansvarlig / Nærmeste leder", tidslinje = "I praktikperiode 3", gennemført = true },
                            new Opgave { kategori = "Faglige mål", beskrivelse = "Selvstændigt kunne køre konference", ansvarlig = "Nærmeste leder/anden", initiator = "Elevansvarlig / Nærmeste leder", tidslinje = "I praktikperiode 3", gennemført = true },
                            new Opgave { kategori = "Faglige mål", beskrivelse = "Selvstændigt kunne køre a la carte", ansvarlig = "Nærmeste leder/anden", initiator = "Elevansvarlig / Nærmeste leder", tidslinje = "I praktikperiode 3", gennemført = true },
                            new Opgave { kategori = "Faglige mål", beskrivelse = "Varebestilling - og planlægning", ansvarlig = "Elevansvarlig / Nærmeste leder", initiator = "Elevansvarlig / Nærmeste leder", tidslinje = "I praktikperiode 3", gennemført = false },
                            new Opgave { kategori = "Faglige mål", beskrivelse = "Nøgletal i afdelingen inklusiv køkkenprocenter", ansvarlig = "Nærmeste leder/anden", initiator = "Elevansvarlig / Nærmeste leder", tidslinje = "I praktikperiode 3", gennemført = false },

                            // Forberedelse til fagprøve
                            new Opgave { kategori = "Klar-parat til fagprøve", beskrivelse = "Gennemgang af indhold til fagprøven", ansvarlig = "Nærmeste leder/anden", initiator = "Elevansvarlig / Nærmeste leder", tidslinje = "I praktikperiode 4", gennemført = false },
                            new Opgave { kategori = "Klar-parat til fagprøve", beskrivelse = "Evt. mangler fra praktikperiode 3 og individuelle ønsker", ansvarlig = "Nærmeste leder/anden", initiator = "Elevansvarlig / Nærmeste leder", tidslinje = "I praktikperiode 4", gennemført = false },
                            new Opgave { kategori = "Klar-parat til fagprøve", beskrivelse = "Karrieresamtale med køkkenchef omkring muligheder", ansvarlig = "Nærmeste leder/anden", initiator = "Elevansvarlig / Nærmeste leder", tidslinje = "I praktikperiode 4", gennemført = false }
                        }
                    }
                }
            };
        }
    }
}