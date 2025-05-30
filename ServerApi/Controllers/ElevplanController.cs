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
        private readonly IElevplan _elevplanService;

        // Dependency injection af service/repository
        public ElevplanController(IElevplan elevplanService)
            => _elevplanService = elevplanService;

        /// <summary>
        /// Returnerer alle elevplaner i systemet.
        /// Bruges typisk til admin-visning eller eksport.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<Elevplan>>> GetAll()
            => Ok(await _elevplanService.GetAllAsync());

        /// <summary>
        /// Henter en specifik elevplan baseret på elevens ID.
        /// Returnerer 404 hvis ikke fundet.
        /// </summary>
        [HttpGet("{elevId}")]
        public async Task<ActionResult<Elevplan?>> GetByElevId(string elevId)
        {
            var plan = await _elevplanService.GetByElevIdAsync(elevId);
            return plan == null ? NotFound() : Ok(plan);
        }

        /// <summary>
        /// Eksporterer hele elevplanen som JSON til fx download eller kopi.
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

        /// <summary>
        /// Opdaterer ét delmål (opgave) i en given praktikperiode for en elev.
        /// Basiskald for check/uncheck i UI.
        /// </summary>
        [HttpPost("{elevId}/opgave")]
        public async Task<IActionResult> UpdateOpgave(
            string elevId,
            [FromBody] UpdateOpgaveRequest req)
        {
            await _elevplanService.UpdateOpgaveAsync(
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
        public class UpdateOpgaveRequest
        {
            public int    PeriodeNummer { get; set; }
            public string Kategori      { get; set; } = string.Empty;
            public string Beskrivelse   { get; set; } = string.Empty;
            public bool   Gennemført    { get; set; }
        }
    }
}
