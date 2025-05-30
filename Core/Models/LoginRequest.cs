namespace Core.Models
{
    /// <summary>
    /// Model brugt til loginformular – sendes til backend ved loginforsøg.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Brugerens e-mailadresse – bruges til at finde bruger i databasen.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Adgangskode angivet af brugeren – skal matche hash i databasen.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}