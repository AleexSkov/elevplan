namespace Core.Models
{
    /// <summary>
    /// Model brugt til loginformular – sendes til backend ved loginforsøg.
    /// </summary>
    public class LoginModel
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
    public class LoginResponse
    {
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Role { get; set; } = default!;
        public bool MustChangePassword { get; set; }
        public string? ElevId { get; set; }
    }
}