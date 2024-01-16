using E2EChatApp.Core.Application.Services;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;
using E2EChatApp.Core.Domain.Exceptions;
using JetBrains.Annotations;
using E2EChatApp.Core.Application.Interfaces;
using E2EEChatApp.UnitTests.Core.Models;
using Moq;
using Newtonsoft.Json;

namespace E2EEChatApp.UnitTests.Core.Application.Services;

[TestSubject(typeof(FriendshipService))]
public class FriendshipServiceTest {
    private readonly Mock<IFriendshipRepository> _friendshipRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly IFriendshipService _friendshipService;
    
    public FriendshipServiceTest()
    {
        _friendshipService = new FriendshipService(_friendshipRepositoryMock.Object, _userRepositoryMock.Object);
    }
    
    public class AddTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1,2 }; 
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class JsonTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var json = File.ReadAllText("../../../data/data.json");
            var testDataList = JsonConvert.DeserializeObject<List<TestData>>(json);

            foreach (var testData in testDataList)
            {
                yield return new object[] { testData.FirstUserId, testData.SecondUserId };
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    #region GetAllFriendshipsByUserId method
    
    [Fact]
    public async Task GetAllFriendshipsByUserId_UserEists_CallsRepository()
    {
        // Arrange
        const int currentUserId = 1;

        _userRepositoryMock.Setup(repo => 
            repo.GetUserById(currentUserId)).ReturnsAsync(new UserModel{Id = currentUserId});
        _friendshipRepositoryMock.Setup(repo =>
            repo.GetFriendshipsByUserId(It.IsAny<int>())).ReturnsAsync(new List<FriendshipModel>());
        // Act
        await _friendshipService.GetAllFriendshipsByUserId(currentUserId);
        // Assert
        _friendshipRepositoryMock.Verify(repo => repo.GetFriendshipsByUserId(currentUserId), Times.Once);
    }
    
    [Fact]
    public async Task GetAllFriendshipsByUserId_UserDoesNotExist_ThrowsException()
    {
        // Arrange
        const int currentUserId = 1;
        _userRepositoryMock.Setup(repo => repo.GetUserById(currentUserId)).ReturnsAsync((UserModel?)null);
        // Act + assert exception
        await Assert.ThrowsAsync<RestException>(async () => await _friendshipService.GetAllFriendshipsByUserId(currentUserId));
        // Assert
        _friendshipRepositoryMock.Verify(repo => repo.GetFriendshipsByUserId(It.IsAny<int>()), Times.Never);
    }
    
    #endregion

    #region CreateFriendship method
    
    [Theory]
    [InlineData(1,2)]
    public async Task CreateFriendship_NoFriendship_CreatesFriendship(int senderIdArg,int receiverIdArg)
    {
        // Arrange
        int senderId = senderIdArg;
        int receiverId = receiverIdArg;
        
        // No friendship exists
        _friendshipRepositoryMock.Setup(repo =>
            repo.GetFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((FriendshipModel?)null);
        // Friendship will be created
        _friendshipRepositoryMock.Setup(repo =>
            repo.CreateFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

        // Act
        await _friendshipService.CreateFriendship(senderId, receiverId);
        
        // Assert
        _friendshipRepositoryMock.Verify(repo => repo.CreateFriendship(senderId, receiverId), Times.Once);
    }
    [Theory]
    [ClassData(typeof(AddTestData))]
    public async Task CreateFriendship_FriendshipExists_ThrowsException(int senderIdArg,int receiverIdArg)
    {
        // Arrange 
        int senderId = senderIdArg;
        int receiverId = receiverIdArg;
        
        var existingFriendship = new FriendshipModel {
            SenderId = receiverId,
            ReceiverId = senderId
        };
        _friendshipRepositoryMock.Setup(repo => repo.GetFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(existingFriendship);
        // Act + assert exception
        await Assert.ThrowsAsync<RestException>(async () => await _friendshipService.CreateFriendship(senderId, receiverId));
        // Assert
        _friendshipRepositoryMock.Verify(repo => repo.CreateFriendship(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
    
    [Theory]
    [ClassData(typeof(JsonTestData))]
    public async Task CreateFriendship_FriendshipRequestForUserExists_IsAccepted(int firstUserId, int secondUserId)
    {
        // Arrange
        int currentUserId = firstUserId;
        int otherUserId = secondUserId;
        
        var existingFriendship = new FriendshipModel {
            SenderId = otherUserId,
            ReceiverId = currentUserId,
            IsPending = true
        };
        _friendshipRepositoryMock.Setup(repo => repo.GetFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(existingFriendship);
        // Act
        await _friendshipService.CreateFriendship(currentUserId, otherUserId);
        // Assert
        _friendshipRepositoryMock.Verify(repo => repo.CreateFriendship(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        _friendshipRepositoryMock.Verify(repo => repo.ConfirmFriendship(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task CreateFriendship_FriendshipRequestFromUserExists_ThrowsException()
    {
        // Arrange
        const int senderId = 1;
        const int receiverId = 2;
        
        var existingFriendship = new FriendshipModel {
            SenderId = senderId,
            ReceiverId = receiverId,
            IsPending = true
        };
        
        _friendshipRepositoryMock.Setup(repo => repo.GetFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(existingFriendship);
        // Act + assert exception
        await Assert.ThrowsAsync<RestException>(async () => await _friendshipService.CreateFriendship(senderId, receiverId));
        // Assert
        _friendshipRepositoryMock.Verify(repo => repo.CreateFriendship(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
    
    #endregion 
    
    #region CancelFriendship method
    
    [Fact]
    public async Task CancelFriendship_ValidInput_CallsRepository()
    {
        // Arrange
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
        // Act
        await _friendshipService.CancelFriendship(currentUserId, otherUserId);
        // Assert
        _friendshipRepositoryMock.Verify(repo => repo.CancelFriendship(currentUserId, otherUserId), Times.Once);
    }
    
    [Fact]
    public async Task CancelFriendship_FriendshipDoesNotExist_ThrowsException()
    {
        // Arrange
        const int senderId = 1;
        const int receiverId = 2;
        
        _friendshipRepositoryMock.Setup(repo => repo.GetFriendship(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((FriendshipModel?)null);
        // Act + assert exception
        await Assert.ThrowsAsync<RestException>(async () => await _friendshipService.CancelFriendship(senderId, receiverId));
        // Assert
        _friendshipRepositoryMock.Verify(repo => repo.CancelFriendship(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
    
    #endregion
}