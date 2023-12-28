namespace E2EChatApp.Core.Domain.Models;

public class FriendshipModel {
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public bool IsPending { get; set; }
}
