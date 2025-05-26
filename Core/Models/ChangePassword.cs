using System.ComponentModel.DataAnnotations;
namespace Core.Models;

public class ChangePassword
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string CurrentPassword { get; set; } = "";  

    [Required, MinLength(6)]
    public string NewPassword { get; set; } = "";      
}
