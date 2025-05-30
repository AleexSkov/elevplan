using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    /// <summary>
    /// ViewModel brugt til at ændre adgangskode for en bruger.
    /// Indeholder e-mail, nuværende og ny adgangskode.
    /// </summary>
    public class ChangePasswordModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";  // Brugerens e-mail (valideres som korrekt e-mailformat)

        [Required]
        public string CurrentPassword { get; set; } = "";  // Eksisterende adgangskode (skal udfyldes)

        [Required, MinLength(6)]
        public string NewPassword { get; set; } = "";  // Ny adgangskode, min. 6 tegn
    }
}