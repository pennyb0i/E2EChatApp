namespace E2EChatApp.Core.Domain.Interfaces;

public interface IFriendshipRepository {
    /// <summary>
    /// Create a friendship request with sender and receiver user
    /// </summary>
    /// <param name="senderId">ID of the user sending the request</param>
    /// <param name="receiverId">ID of the user receiving the request</param>
    /// <returns></returns>
    Task<bool> CreateFriendship(int senderId, int receiverId);
    
    /// <summary>
    /// Confirm a friendship request between two users
    /// </summary>
    /// <param name="senderId"></param>
    /// <param name="receiverId"></param>
    /// <returns></returns>
    Task<bool> ConfirmFriendship(int senderId, int receiverId);
    
    /// <summary>
    /// Cancel a friendship request between two users
    /// </summary>
    /// <param name="senderId"></param>
    /// <param name="receiverId"></param>
    /// <returns></returns>
    Task<bool> CancelFriendship(int senderId, int receiverId);
}
