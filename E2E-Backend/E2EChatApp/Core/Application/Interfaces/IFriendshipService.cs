using E2EChatApp.Core.Domain.Models;
namespace E2EChatApp.Core.Application.Interfaces;

public interface IFriendshipService {
    /// <summary>
    /// Get all friendships/friend requests for userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<FriendshipModel>> GetAllFriendshipsByUserId(int userId);
    
    /// <summary>
    /// Create, or Confirm a friendship request between two users
    /// </summary>
    /// <param name="currentUserId"></param>
    /// <param name="otherUserId"></param>
    /// <returns></returns>
    Task<bool> CreateFriendship(int currentUserId, int otherUserId);
    
    /// <summary>
    /// Cancel a friendship request between two users
    /// </summary>
    /// <param name="currentUserId"></param>
    /// <param name="otherUserId"></param>
    /// <returns></returns>
    Task<bool> CancelFriendship(int currentUserId, int otherUserId);
}
