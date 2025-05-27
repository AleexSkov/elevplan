namespace Core.Models;

public class LoginResponse
{
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool MustChangePassword { get; set; }
        public string Name { get; set; } = string.Empty;
    

}