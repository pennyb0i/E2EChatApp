using E2EChatApp.Core.Domain.Dtos;
namespace E2EChatApp.Core.Domain.Models;

public class FriendshipModel {
    public int SenderId { get; set; }
    public UserDto Sender { get; set; }
    public int ReceiverId { get; set; }
    public UserDto Receiver { get; set; }
    public bool IsPending { get; set; }
}
