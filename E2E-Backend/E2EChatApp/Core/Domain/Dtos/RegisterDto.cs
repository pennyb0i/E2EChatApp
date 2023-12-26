namespace E2EChatApp.Core.Domain.Dtos;

public class RegisterDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
    public string PublicKey { get; set; }
}