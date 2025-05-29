using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using ServerApi.Interface;

namespace ServerApi.Controllers
{
    [ApiController]
    [Route("api/elevplaner/{elevId}/perioder/{periodeNummer}/delmaal")]
    public class DelmaalController : ControllerBase
    {
        private readonly IElevplan _repository;

        public DelmaalController(IElevplan repository)
        {
            _repository = repository;
        }

        // GET: api/elevplaner/{elevId}/perioder/{periodeNummer}/delmaal
        [HttpGet]
        public async Task<ActionResult<List<Delmaal>>> GetListe(string elevId, int periodeNummer)
        {
            var liste = await _repository.GetDelmaalListeAsync(elevId, periodeNummer);
            return Ok(liste);
        }

        // GET: api/elevplaner/{elevId}/perioder/{periodeNummer}/delmaal/{delmaalId}
        [HttpGet("{delmaalId}")]
        public async Task<ActionResult<Delmaal>> Get(string elevId, int periodeNummer, Guid delmaalId)
        {
            var delmaal = await _repository.GetDelmaalAsync(elevId, periodeNummer, delmaalId);
            if (delmaal == null)
            {
                return NotFound();
            }
            return Ok(delmaal);
        }

        // POST: api/elevplaner/{elevId}/perioder/{periodeNummer}/delmaal
        [HttpPost]
        public async Task<ActionResult<Delmaal>> Post(string elevId, int periodeNummer, [FromBody] Delmaal delmaal)
        {
            var created = await _repository.AddDelmaalAsync(elevId, periodeNummer, delmaal);
            return CreatedAtAction(nameof(Get), new { elevId, periodeNummer, delmaalId = created.DelmaalId }, created);
        }

        // PUT: api/elevplaner/{elevId}/perioder/{periodeNummer}/delmaal/{delmaalId}
        [HttpPut("{delmaalId}")]
        public async Task<IActionResult> Put(string elevId, int periodeNummer, Guid delmaalId, [FromBody] Delmaal updated)
        {
            var result = await _repository.UpdateDelmaalAsync(elevId, periodeNummer, delmaalId, updated);
            if (result == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/elevplaner/{elevId}/perioder/{periodeNummer}/delmaal/{delmaalId}
        [HttpDelete("{delmaalId}")]
        public async Task<IActionResult> Delete(string elevId, int periodeNummer, Guid delmaalId)
        {
            var success = await _repository.DeleteDelmaalAsync(elevId, periodeNummer, delmaalId);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}