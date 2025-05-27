using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ServerApi.Interface;
using Core.Models;
using System.Threading.Tasks;

namespace ServerApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAppUser _appUserRepo;
        private readonly IElevplan _elevplanService;

        public AuthController(IAppUser appUserRepo, IElevplan elevplanService)
        {
            _appUserRepo    = appUserRepo;
            _elevplanService = elevplanService;
        }

        public class RegisterResponse
        {
            public string Message { get; set; } = default!;
            public string Password { get; set; } = default!;
            public string? ElevId { get; set; }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AppUser newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.Email) || string.IsNullOrWhiteSpace(newUser.PasswordHash))
                return BadRequest("Email og password er påkrævet.");

            if (await _appUserRepo.EmailExistsAsync(newUser.Email))
                return Conflict("En bruger med denne email findes allerede.");

            // Hash password
            var hasher = new PasswordHasher<AppUser>();
            var rawPassword = newUser.PasswordHash;
            newUser.PasswordHash = hasher.HashPassword(newUser, rawPassword);
            newUser.MustChangePassword = true;
            newUser.Id = MongoDB.Bson.ObjectId.GenerateNewId();
            newUser.CreatedAt = DateTime.UtcNow;

            // Gem bruger
            await _appUserRepo.CreateAsync(newUser);

            string? assignedElevId = null;

            // Opret elevplan hvis det er en elev
            if (newUser.Role == "Elev")
            {
                var template = await _elevplanService.GetTemplateAsync();
                if (template != null)
                {
                    var newPlan = new Elevplan
                    {
                        // ElevplanRepository genererer selv unikt int Id
                        ElevId         = newUser.Id.ToString(),
                        ElevNavn       = newUser.Name ?? string.Empty,
                        Aftaleform     = template.Aftaleform,
                        Skole          = template.Skole,
                        Praktikperioder = template.Praktikperioder,
                        OprettetDato   = DateTime.UtcNow,
                        OpdateretDato  = DateTime.UtcNow
                    };
                    await _elevplanService.CreateAsync(newPlan);
                    assignedElevId = newPlan.ElevId;
                }
            }

            // Returnér besked, kode og evt. ElevId
            var result = new RegisterResponse
            {
                Message = "Bruger oprettet",
                Password = rawPassword,
                ElevId = assignedElevId
            };
            return Ok(result);
        }
    }
}
