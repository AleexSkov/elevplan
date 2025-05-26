using System;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServerApi.Interface;
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

            // Hash adgangskoden her
            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<AppUser>();
            newUser.PasswordHash = hasher.HashPassword(newUser, newUser.PasswordHash);
            newUser.MustChangePassword = true;
            newUser.Id = MongoDB.Bson.ObjectId.GenerateNewId();
            newUser.CreatedAt = DateTime.UtcNow;

            await _appUserRepo.CreateAsync(newUser);

            return Ok(new { Message = "Bruger oprettet", Password = "[GENERERET]" }); // Du kan fjerne password hvis ikke nødvendig
        }


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
            HttpContext.Session.SetString("user_role", user.Role);

            return Ok(new
            {
                Email = user.Email,
                Name = user.Name,
                Role = user.Role,
                MustChangePassword = user.MustChangePassword
            });
        }

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

            user.PasswordHash = hasher.HashPassword(user, model.NewPassword);
            user.MustChangePassword = false;

            await _appUserRepo.UpdateAsync(user.Id, user);


            return Ok("Adgangskode opdateret");
        }



    }
}
