using E2EChatApp.Core.Domain.Enums;
namespace E2EChatApp.Core.Domain.Dtos;

public class UserDto {
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public string? PublicKey { get; set; }
    public FriendshipStatus? FriendshipStatus { get; set; }
}
