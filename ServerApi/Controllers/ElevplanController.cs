using Microsoft.AspNetCore.Mvc;
using ServerApi.Interface;
using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<ActionResult<List<Elevplan>>> GetAll()
        {
            var elevplaner = await _elevplanService.GetAllAsync();
            return Ok(elevplaner);
        }
    }
}