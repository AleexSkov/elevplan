using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Interface;
using Core.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace ServerApi.Controllers
{
    [ApiController]
    [Route("api/auth")] // 👈 Fast og tydelig
    public class AuthController : ControllerBase
    {
        private readonly IAppUser _appUserRepo;

        public AuthController(IAppUser appUserRepo)
        {
            _appUserRepo = appUserRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AppUser newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.Email) || string.IsNullOrWhiteSpace(newUser.PasswordHash))
                return BadRequest("Email og password er påkrævet.");

            if (await _appUserRepo.EmailExistsAsync(newUser.Email))
                return Conflict("En bruger med denne email findes allerede.");

            newUser.Id = ObjectId.GenerateNewId();
            newUser.CreatedAt = DateTime.UtcNow;

            await _appUserRepo.CreateAsync(newUser);
            return CreatedAtAction(nameof(GetByEmail), new { email = newUser.Email }, newUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _appUserRepo.GetByEmailAsync(request.Email);
            if (user == null || user.PasswordHash != request.Password)
                return Unauthorized("Ugyldig email eller adgangskode");

            HttpContext.Session.SetString("user_email", user.Email);
            HttpContext.Session.SetString("user_role", user.Role);

            return Ok("Login godkendt");
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _appUserRepo.GetByEmailAsync(email);
            if (user == null)
                return NotFound("Bruger ikke fundet");

            return Ok(user);
        }
    }
}
