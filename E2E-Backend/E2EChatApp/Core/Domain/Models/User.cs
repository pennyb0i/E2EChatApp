namespace E2EChatApp.Core.Domain.Models;

public class User {
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PublicKey { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}
    