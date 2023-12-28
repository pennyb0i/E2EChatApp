using System.Net;
using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Domain.Exceptions;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;
namespace E2EChatApp.Core.Application.Services;

public class FriendshipService : IFriendshipService{
    private readonly IFriendshipRepository _friendshipRepository;

    public FriendshipService(IFriendshipRepository friendshipRepository)
    {
        _friendshipRepository = friendshipRepository;
    }

    public async Task<List<FriendshipModel>> GetAllFriendshipsByUserId(int userId)
    {
        return await _friendshipRepository.GetFriendshipsByUserId(userId);
    }

    public async Task<bool> CreateFriendship(int currentUserId, int otherUserId)
    {
        // Get a pending friend request/friendship, if there is one
        var existingFriendship = await _friendshipRepository.GetFriendship(otherUserId, currentUserId);

        // No request => Create a friend request
        if ( existingFriendship is null ) {
            return await _friendshipRepository.CreateFriendship(currentUserId, otherUserId);
        }
        // Friendship exists => Check if it's pending
        if (existingFriendship.IsPending) {
            // Friendship request is sent by the other user => Accept the friend request
            if (existingFriendship.SenderId != currentUserId) {
                return await _friendshipRepository.ConfirmFriendship(otherUserId,currentUserId);
            }
            throw new RestException(HttpStatusCode.BadRequest, "You have already sent a friend request to this user!");
        }
        throw new RestException(HttpStatusCode.BadRequest, "You are already friends with this user!");
    }
    
    public async Task<bool> CancelFriendship(int currentUserId, int otherUserId)
    {
        // Get a friendship/friend request , if there is one
        var existingFriendship = await _friendshipRepository.GetFriendship(otherUserId, currentUserId);
        if (existingFriendship is not null) {
            return await _friendshipRepository.CancelFriendship(existingFriendship.SenderId, existingFriendship.ReceiverId);
        }
        throw new RestException(HttpStatusCode.NotFound, "No friendship or friend request found for this user");
    }
}
