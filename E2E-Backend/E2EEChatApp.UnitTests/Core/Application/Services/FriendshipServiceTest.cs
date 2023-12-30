using E2EChatApp.Core.Application.Services;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;
using E2EChatApp.Core.Domain.Exceptions;
using JetBrains.Annotations;
using E2EChatApp.Core.Application.Interfaces;
using Moq;

namespace E2EEChatApp.UnitTests.Core.Application.Services;

[TestSubject(typeof(FriendshipService))]
public class FriendshipServiceTest {
    private readonly Mock<IFriendshipRepository> _friendshipRepositoryMock = new();
    private readonly IFriendshipService _friendshipService;
    
    public FriendshipServiceTest() // dependency injection via test suite's constructor  
    {
        _friendshipService = new FriendshipService(_friendshipRepositoryMock.Object);
    }
    
    // TestCases for GetAllFriendshipsByUserId method
    [Fact]
    public async Task GetAllFriendshipsByUserId_ValidInput_CallsRepository()
    {
        var userId = 1;
        _friendshipRepositoryMock.Setup(repo =>
            repo.GetFriendshipsByUserId(It.IsAny<int>())).ReturnsAsync(new List<FriendshipModel>());

        await _friendshipService.GetAllFriendshipsByUserId(userId);

        _friendshipRepositoryMock.Verify(repo => repo.GetFriendshipsByUserId(userId), Times.Once);
    }

    #region TestCases for CreateFriendship method
    
    [Fact]
    public async Task CreateFriendship_NoFriendship_CreatesFriendship()
    {
        const int senderId = 1;
        const int receiverId = 2;
        
        // No friendship exists
        _friendshipRepositoryMock.Setup(repo =>
            repo.GetFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((FriendshipModel?)null);
        // Friendship wil be created
        _friendshipRepositoryMock.Setup(repo =>
            repo.CreateFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

        await _friendshipService.CreateFriendship(senderId, receiverId);

        _friendshipRepositoryMock.Verify(repo => repo.CreateFriendship(senderId, receiverId), Times.Once);
    }
    [Fact]
    public async Task CreateFriendship_FriendshipExists_ThrowsException()
    {
        const int senderId = 1;
        const int receiverId = 2;
        // Friendship exists
        var existingFriendship = new FriendshipModel {
            SenderId = receiverId,
            ReceiverId = senderId
        };
        _friendshipRepositoryMock.Setup(repo => repo.GetFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(existingFriendship);
        await Assert.ThrowsAsync<RestException>(async () =>
        {
            await _friendshipService.CreateFriendship(senderId, receiverId);
        });
        _friendshipRepositoryMock.Verify(repo => repo.CreateFriendship(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
    
    [Fact]
    public async Task CreateFriendship_FriendshipRequestForUserExists_IsAccepted()
    {
        const int currentUserId = 1;
        const int otherUserId = 2;
        // Friendship exists
        var existingFriendship = new FriendshipModel {
            SenderId = otherUserId,
            ReceiverId = currentUserId,
            IsPending = true
        };
        _friendshipRepositoryMock.Setup(repo => repo.GetFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(existingFriendship);
        
        await _friendshipService.CreateFriendship(currentUserId, otherUserId);
        _friendshipRepositoryMock.Verify(repo => repo.CreateFriendship(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        _friendshipRepositoryMock.Verify(repo => repo.ConfirmFriendship(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task CreateFriendship_FriendshipRequestFromUserExists_ThrowsException()
    {
        const int senderId = 1;
        const int receiverId = 2;
        
        var existingFriendship = new FriendshipModel {
            SenderId = senderId,
            ReceiverId = receiverId,
            IsPending = true
        };
        
        _friendshipRepositoryMock.Setup(repo => repo.GetFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(existingFriendship);
    
        await Assert.ThrowsAsync<RestException>(async () =>
        {
            await _friendshipService.CreateFriendship(senderId, receiverId);
        });
    
        _friendshipRepositoryMock.Verify(repo => repo.CreateFriendship(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
    
    #endregion 
    
    // TestCases for CancelFriendship method
    [Fact]
    public async Task CancelFriendship_ValidInput_CallsRepository()
    {
        const int currentUserId = 1;
        const int otherUserId = 2;
        var existingFriendship = new FriendshipModel {
            SenderId = currentUserId,
            ReceiverId = otherUserId,
            IsPending = true
        };
        _friendshipRepositoryMock.Setup(repo =>
            repo.GetFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(existingFriendship);
        _friendshipRepositoryMock.Setup(repo =>
            repo.CancelFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

        await _friendshipService.CancelFriendship(currentUserId, otherUserId);

        _friendshipRepositoryMock.Verify(repo => repo.CancelFriendship(currentUserId, otherUserId), Times.Once);
    }
    
    [Fact]
    public async Task CancelFriendship_FriendshipDoesNotExist_ThrowsException()
    {
        const int senderId = 1;
        const int receiverId = 2;
        // Friendship does not exist
        _friendshipRepositoryMock.Setup(repo => repo.GetFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((FriendshipModel?)null);
        await Assert.ThrowsAsync<RestException>(async () =>
        {
            await _friendshipService.CancelFriendship(senderId, receiverId);
        });
        _friendshipRepositoryMock.Verify(repo => repo.CancelFriendship(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
}