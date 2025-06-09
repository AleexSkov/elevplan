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
        private readonly IElevplan _repository; //et interface-felt til at hente og gemme data om elevplaner 
        
        public DelmaalController(IElevplan repository) 
        {
            _repository = repository;  // DI: injection af et objekt, der implementerer IElevplan
        }

        /// <summary>
        /// Henter listen af delmål for en bestemt elev og praktikperiode.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<Delmaal>>> GetListe(string elevId, int periodeNummer)
        {
            var liste = await _repository.GetDelmaalListeAsync(elevId, periodeNummer);
            return Ok(liste);
        }

        /// <summary>
        /// Henter ét specifikt delmål baseret på elevId, periode og delmålId.
        /// </summary>
        [HttpGet("{delmaalId}")]
        public async Task<ActionResult<Delmaal>> Get(string elevId, int periodeNummer, Guid delmaalId)
        {
            // kald repositpry for at hente et delmaal-objekt med det specifikke delmål
            var delmaal = await _repository.GetDelmaalAsync(elevId, periodeNummer, delmaalId);
            if (delmaal == null)
            {
                return NotFound();
            }
            return Ok(delmaal);
        }

        /// <summary>
        /// Opretter et nyt delmål til en bestemt periode for en elev.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Delmaal>> Post(string elevId, int periodeNummer, [FromBody] Delmaal delmaal)
        {
            // kalder repository for at oprette/føre et nyt delmaal til databasen
            var created = await _repository.AddDelmaalAsync(elevId, periodeNummer, delmaal);
            return CreatedAtAction(nameof(Get), new { elevId, periodeNummer, delmaalId = created.DelmaalId }, created); // retunerer det oprettede delmål-objekt
        }

        /// <summary>
        /// Opdaterer et eksisterende delmål for en elev i en given periode.
        /// </summary>
        [HttpPut("{delmaalId}")]
        public async Task<IActionResult> Put(string elevId, int periodeNummer, Guid delmaalId, [FromBody] Delmaal updated)
        {
            // kalder repo for at opdatere delmålet i databasen
            var result = await _repository.UpdateDelmaalAsync(elevId, periodeNummer, delmaalId, updated);
            if (result == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Sletter et delmål fra en elevs praktikplan.
        /// </summary>
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
