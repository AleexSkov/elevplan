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
        // 1) felter til at holde på repository-objekter (interfaceses) som injiceres via konstruktøren nedenfor
        private readonly IAppUser _appUserRepo;
        private readonly IElevplan _elevplanService;

        // Dependency injection af bruger- og elevplanrepositories
       
        // konstruktør med parametre AppUserRepository og Elevplanservice
        public AuthController(IAppUser appUserRepo, IElevplan elevplanService) // konstruktør: her injiceres dependencies (IAppUser og IElevplan) automatisk
        {
            _appUserRepo = appUserRepo; // tildeler det injicerede repository til det private 
            _elevplanService = elevplanService; // tildeler den injicerede elevplan-service til feltet 
        }

        // Bruges til at returnere ekstra info ved brugeroprettelse
     

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

            var rawPw = newUser.PasswordHash; // gemmer den rå adgangskode i en lokal variabel
            var hasher = new PasswordHasher<AppUser>(); // opretter en passwordHasher til at hashe adgangskode 
            newUser.PasswordHash = hasher.HashPassword(newUser, rawPw); // hash adgangskoden og gem i newUser.passwordHash
            newUser.MustChangePassword = true; // Marker, at brugeren skal skifte kode 
            newUser.Id = MongoDB.Bson.ObjectId.GenerateNewId(); //  Genererer et nyt ObjectId fra MongoDB-driveren (unik ID)
            newUser.CreatedAt = DateTime.UtcNow; // sætter oprettelsesdato

            // Gem brugeren i databasen
            await _appUserRepo.CreateAsync(newUser); // kalder repository 

            string? elevId = null; //  // Initialiserer elevId til null – bruges kun hvis role == “Elev”

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
                    await _elevplanService.CreateAsync(plan); // gemmer den nye elevplan i databasen via IElevplan-servicen
                    elevId = plan.ElevId; // gemmer elevId i lokal variabel, som sendes til frontend
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
            // Forsøg at hente brugeren (AppUser) ud fra e-mail i databasen
            var user = await _appUserRepo.GetByEmailAsync(req.Email);
            if (user == null)
                return Unauthorized("Ugyldig email eller adgangskode");

            // opret PasswordHasher og tjek adgangskode mod den hashede version
            var hasher = new PasswordHasher<AppUser>();
            // VerifierHashedPassword retunerer en (success/failed)
            var res = hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
            if (res == PasswordVerificationResult.Failed)
                return Unauthorized("Ugyldig email eller adgangskode");

            // Sæt session-variabler (bruges til login-status)
            HttpContext.Session.SetString("user_email", user.Email);
            HttpContext.Session.SetString("user_role", user.Role);

            string? elevId = null;
            // STEP 2: Validerer bruger og henter evt. elevplan -> næste stop er LoginForm for at gemme elevId
            // Find elevId hvis brugeren er elev, hent elevplan og udtræk elevid 
            if (user.Role == "Elev")
            {
                // Hent elevplan baseret på brugerens ID
                var plan = await _elevplanService.GetByElevIdAsync(user.Id.ToString());
                elevId = plan?.ElevId;
            }

            // disse data bliver i blazor-klient brugt til at gemme i localStorage
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
            // finder brugren i databasen 
            var user = await _appUserRepo.GetByEmailAsync(m.Email);
            if (user == null)
                return NotFound("Bruger ikke fundet");

            // opretter en passwordHasher til at samlinge/hash 
            var hasher = new PasswordHasher<AppUser>();
            
            // verificerer, at den indtastede "currentpassword" matcher den hashede version
            var res = hasher.VerifyHashedPassword(user, user.PasswordHash, m.CurrentPassword);
            if (res == PasswordVerificationResult.Failed)
                return BadRequest(new { error = "Nuværende adgangskode er forkert" });

            // Hasher den nye kode og gemmer den i brugerobjektet
            user.PasswordHash = hasher.HashPassword(user, m.NewPassword);
            user.MustChangePassword = false;
            // opdaterer det eksisterende brugerobjekt i databasen med ny kode
            await _appUserRepo.UpdateAsync(user.Id, user);

            return Ok("Adgangskode opdateret");
        }
    }
}
