using Microsoft.AspNetCore.Mvc;
using Core.Models;
using ServerApi.Repository;

namespace ServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElevplanController : ControllerBase
    {
        private readonly ElevplanRepository _repository;

        public ElevplanController(ElevplanRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Elevplan>>> GetAll()
        {
            try
            {
                var elevplans = await _repository.GetAllAsync();
                return Ok(elevplans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}