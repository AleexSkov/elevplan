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
        private readonly IAppUser  _appUserRepo;
        private readonly IElevplan _elevplanService;

        public AuthController(IAppUser appUserRepo, IElevplan elevplanService)
        {
            _appUserRepo     = appUserRepo;
            _elevplanService = elevplanService;
        }

        // DTO til registrering (kan også bruge AppUser direkte men ofte bedre med en request-klasse)
        public class RegisterRequest
        {
            public string Email    { get; set; } = default!;
            public string Password { get; set; } = default!;
            public string Name     { get; set; } = default!;
            public string Role     { get; set; } = "Elev";
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Email og password er påkrævet.");

            if (await _appUserRepo.EmailExistsAsync(req.Email))
                return Conflict("En bruger med denne email findes allerede.");

            var user = new AppUser
            {
                Id               = MongoDB.Bson.ObjectId.GenerateNewId(),
                Email            = req.Email,
                Name             = req.Name,
                Role             = req.Role,
                MustChangePassword = true,
            };

            // Hash password
            var hasher = new PasswordHasher<AppUser>();
            user.PasswordHash = hasher.HashPassword(user, req.Password);

            await _appUserRepo.CreateAsync(user);

            // Hvis det er en Elev, opret en kopi af template
            if (user.Role == "Elev")
            {
                var template = await _elevplanService.GetTemplateAsync();
                if (template != null)
                {
                    var newPlan = new Elevplan
                    {
                        ElevId        = user.Id.ToString(),
                        ElevNavn      = user.Name!,
                        Aftaleform    = template.Aftaleform,
                        Skole         = template.Skole,
                        Praktikperioder   = template.Praktikperioder,
                        OprettetDato  = DateTime.UtcNow,
                        OpdateretDato = DateTime.UtcNow
                    };
                    await _elevplanService.CreateAsync(newPlan);
                }
            }

            return Ok(new
            {
                Message = "Bruger oprettet",
                ElevId  = user.Role == "Elev" ? user.Id.ToString() : null
            });
        }

        public class LoginRequest
        {
            public string Email    { get; set; } = default!;
            public string Password { get; set; } = default!;
        }

        public class LoginResponse
        {
            public string  Email              { get; set; } = default!;
            public string  Name               { get; set; } = default!;
            public string  Role               { get; set; } = default!;
            public bool    MustChangePassword { get; set; }
            public string? ElevId             { get; set; }
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _appUserRepo.GetByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized("Ugyldig email eller adgangskode");

            var hasher = new PasswordHasher<AppUser>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Ugyldig email eller adgangskode");

            HttpContext.Session.SetString("user_email", user.Email);
            HttpContext.Session.SetString("user_role",  user.Role);

            // Hent ElevId hvis rollen er Elev
            string? elevId = null;
            if (user.Role == "Elev")
            {
                var plan = await _elevplanService.GetByElevIdAsync(user.Id.ToString());
                elevId = plan?.ElevId;
            }

            return Ok(new LoginResponse
            {
                Email              = user.Email,
                Name               = user.Name ?? "",
                Role               = user.Role,
                MustChangePassword = user.MustChangePassword,
                ElevId             = elevId
            });
        }

        // POST api/auth/change-password
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var user = await _appUserRepo.GetByEmailAsync(model.Email);
            if (user == null)
                return NotFound("Bruger ikke fundet");

            var hasher = new PasswordHasher<AppUser>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, model.CurrentPassword);
            if (result == PasswordVerificationResult.Failed)
                return BadRequest(new { error = "Nuværende adgangskode er forkert" });

            user.PasswordHash       = hasher.HashPassword(user, model.NewPassword);
            user.MustChangePassword = false;
            await _appUserRepo.UpdateAsync(user.Id, user);

            return Ok("Adgangskode opdateret");
        }
    }
}
