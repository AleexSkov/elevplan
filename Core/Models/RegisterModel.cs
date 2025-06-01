namespace Core.Models;

public class RegisterResponse
{
    public string Message { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string? ElevId { get; set; }
}