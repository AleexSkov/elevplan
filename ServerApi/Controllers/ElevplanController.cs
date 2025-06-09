using Microsoft.AspNetCore.Mvc;
using ServerApi.Interface;
using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerApi.Controllers
{
    [ApiController]
    [Route("api/elevplaner")]
    public class ElevplanController : ControllerBase 
    {
        private readonly IElevplan _elevplanService; // Felt/variable som gemmer det interface der håndterer data (DI)

        // Dependency injection af service/repository
        public ElevplanController(IElevplan elevplanService) // Konstruktør: modtager et objekt der implementerer IElevplan
            => _elevplanService = elevplanService; // Gemmer det modtagne objekt i feltet

        /// <summary>
        /// Returnerer alle elevplaner i systemet.
        /// Bruges typisk til admin-visning eller eksport.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<Elevplan>>> GetAll()
            => Ok(await _elevplanService.GetAllAsync()); // kalder metoden GetAllAsync og retunerer listen 

        /// <summary>
        /// Henter en specifik elevplan baseret på elevens ID.
        /// Returnerer 404 hvis ikke fundet.
        /// </summary>
        // STEP 5: Sender forespørgsel videre via HttpGet til repository -> se ElevplanRepository.GetByElevIdAsync
        [HttpGet("{elevId}")]
        public async Task<ActionResult<Elevplan?>> GetByElevId(string elevId)
        {
            // Henter planen for den elev 
            var plan = await _elevplanService.GetByElevIdAsync(elevId);
            //STEP 7 : retunere plan eller 404, nu er dataen loaded på dashboardelev
            return plan == null ? NotFound() : Ok(plan);
        }

       /* /// <summary>
        /// mulighed for at kunne Eksporterer hele elevplanen som JSON til fx download eller kopi. ( ikke funktionel)
        /// Samme som GetByElevId men med en anden rute.
        /// </summary>
        [HttpGet("{elevId}/export")]
        public async Task<IActionResult> ExportByElevId(string elevId)
        {
            var plan = await _elevplanService.GetByElevIdAsync(elevId);
            if (plan == null)
                return NotFound();

            return Ok(plan); // Returner hele planen i JSON-format
        }
        */

        /// <summary>
        /// Opdaterer ét delmål (opgave) i en given praktikperiode for en elev.
        /// Basiskald for check/uncheck i UI.
        /// </summary>
        [HttpPost("{elevId}/opgave")]
        public async Task<IActionResult> UpdateOpgave(
            string elevId,
            [FromBody] UpdateOpgaveRequest req) // data fra body kommer ind som objekt
        {
            await _elevplanService.UpdateOpgaveAsync( // kalder metode der opdaterer opgaven
                elevId,
                req.PeriodeNummer,
                req.Kategori,
                req.Beskrivelse,
                req.Gennemført);
            return NoContent(); 
        }

        /// <summary>
        /// Request-body til UpdateOpgave – bruges til at identificere og opdatere ét delmål.
        /// </summary>
    
       
    }
}
