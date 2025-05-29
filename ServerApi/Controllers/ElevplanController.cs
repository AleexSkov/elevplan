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

        public ElevplanController(IElevplan elevplanService)
            => _elevplanService = elevplanService;

        // GET api/elevplaner
        [HttpGet]
        public async Task<ActionResult<List<Elevplan>>> GetAll()
            => Ok(await _elevplanService.GetAllAsync());

        // GET api/elevplaner/{elevId}
        [HttpGet("{elevId}")]
        public async Task<ActionResult<Elevplan?>> GetByElevId(string elevId)
        {
            var plan = await _elevplanService.GetByElevIdAsync(elevId);
            return plan == null ? NotFound() : Ok(plan);
        }

        // NYT: GET api/elevplaner/{elevId}/export
        [HttpGet("{elevId}/export")]
        public async Task<IActionResult> ExportByElevId(string elevId)
        {
            var plan = await _elevplanService.GetByElevIdAsync(elevId);
            if (plan == null)
                return NotFound();

            // Returner hele planen som JSON
            return Ok(plan);
        }

        // POST api/elevplaner/{elevId}/opgave
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

        public class UpdateOpgaveRequest
        {
            public int    PeriodeNummer { get; set; }
            public string Kategori      { get; set; } = string.Empty;
            public string Beskrivelse   { get; set; } = string.Empty;
            public bool   Gennemført    { get; set; }
        }
    }
}
