using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interface;
using Core.Models;

namespace ServerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElevplanController : ControllerBase
    {
        private readonly IElevplan _elevplanService;

        public ElevplanController(IElevplan elevplanService)
        {
            _elevplanService = elevplanService;
        }

        [HttpGet("raw")] // ✅ Tilføj endpoint for raw data
        public async Task<ActionResult<List<Elevplan>>> GetAll()
        {
            var elevplaner = await _elevplanService.GetAllAsync();
            return Ok(elevplaner);
        }
    }
}