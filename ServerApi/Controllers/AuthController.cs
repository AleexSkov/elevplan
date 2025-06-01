using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Core.Models;
using ServerApi.Interface;

namespace ServerApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAppUser _appUserRepo;
        private readonly IElevplan _elevplanService;

        // Dependency injection af bruger- og elevplanrepositories
        public AuthController(IAppUser appUserRepo, IElevplan elevplanService)
        {
            _appUserRepo = appUserRepo;
            _elevplanService = elevplanService;
        }

        // Bruges til at returnere ekstra info ved brugeroprettelse
        public class RegisterResponse
        {
            public string Message { get; set; } = default!;
            public string Password { get; set; } = default!;
            public string? ElevId { get; set; }
        }

        /// <summary>
        /// Endpoint til at registrere en ny bruger (Elev/Admin).
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AppUser newUser)
        {
            // Valider at e-mail og adgangskode er givet
            if (string.IsNullOrWhiteSpace(newUser.Email)
             || string.IsNullOrWhiteSpace(newUser.PasswordHash))
                return BadRequest("Email og password er påkrævet.");

            // Tjek om brugeren allerede eksisterer
            if (await _appUserRepo.EmailExistsAsync(newUser.Email))
                return Conflict("En bruger med denne email findes allerede.");

            var rawPw = newUser.PasswordHash;
            var hasher = new PasswordHasher<AppUser>();
            newUser.PasswordHash = hasher.HashPassword(newUser, rawPw);
            newUser.MustChangePassword = true;
            newUser.Id = MongoDB.Bson.ObjectId.GenerateNewId();
            newUser.CreatedAt = DateTime.UtcNow;

            // Gem brugeren i databasen
            await _appUserRepo.CreateAsync(newUser);

            string? elevId = null;

            // Hvis brugeren er Elev, opret en tom elevplan baseret på en skabelon
            if (newUser.Role == "Elev")
            {
                var template = await _elevplanService.GetTemplateAsync();
                if (template != null)
                {
                    var plan = new Elevplan
                    {
                        ElevId = newUser.Id.ToString(),
                        ElevNavn = newUser.Name ?? "",
                        Aftaleform = template.Aftaleform,
                        Skole = template.Skole,
                        Praktikperioder = template.Praktikperioder,
                        OprettetDato = DateTime.UtcNow,
                        OpdateretDato = DateTime.UtcNow
                    };
                    await _elevplanService.CreateAsync(plan);
                    elevId = plan.ElevId;
                }
            }

            // Returnér adgangskode og evt. elevId (bruges til frontend visning)
            return Ok(new RegisterResponse
            {
                Message = "Bruger oprettet",
                Password = rawPw,
                ElevId = elevId
            });
        }

       

        /// <summary>
        /// Login endpoint – validerer bruger og returnerer info.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel req)
        {
            var user = await _appUserRepo.GetByEmailAsync(req.Email);
            if (user == null)
                return Unauthorized("Ugyldig email eller adgangskode");

            var hasher = new PasswordHasher<AppUser>();
            var res = hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
            if (res == PasswordVerificationResult.Failed)
                return Unauthorized("Ugyldig email eller adgangskode");

            // Sæt session-variabler (bruges til login-status)
            HttpContext.Session.SetString("user_email", user.Email);
            HttpContext.Session.SetString("user_role", user.Role);

            string? elevId = null;

            // Find elevId hvis brugeren er elev
            if (user.Role == "Elev")
            {
                var plan = await _elevplanService.GetByElevIdAsync(user.Id.ToString());
                elevId = plan?.ElevId;
            }

            return Ok(new LoginResponse
            {
                Email = user.Email,
                Name = user.Name ?? "",
                Role = user.Role,
                MustChangePassword = user.MustChangePassword,
                ElevId = elevId
            });
        }

        /// <summary>
        /// Endpoint til at ændre adgangskode, kræver korrekt eksisterende kode.
        /// </summary>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel m)
        {
            var user = await _appUserRepo.GetByEmailAsync(m.Email);
            if (user == null)
                return NotFound("Bruger ikke fundet");

            var hasher = new PasswordHasher<AppUser>();
            var res = hasher.VerifyHashedPassword(user, user.PasswordHash, m.CurrentPassword);
            if (res == PasswordVerificationResult.Failed)
                return BadRequest(new { error = "Nuværende adgangskode er forkert" });

            user.PasswordHash = hasher.HashPassword(user, m.NewPassword);
            user.MustChangePassword = false;
            await _appUserRepo.UpdateAsync(user.Id, user);

            return Ok("Adgangskode opdateret");
        }
    }
}
