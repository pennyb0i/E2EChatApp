using E2EChatApp.Core.Application.Services;
using E2EChatApp.Core.Domain.Exceptions;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;
using JetBrains.Annotations;
using Moq;

namespace E2EEChatApp.UnitTests.Core.Application.Services;

[TestSubject(typeof(MessageService))]
public class MessageServiceTest {
    private readonly MessageService _messageService;
    private readonly Mock<IMessageRepository> _messageRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public MessageServiceTest()
    {
        _messageRepositoryMock = new Mock<IMessageRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _messageService = new MessageService(_messageRepositoryMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetMessages_UserNotFound_ThrowsRestException()
    {
        // Arrange
        var currentUserId = 1;
        var otherUserId = 2;
        _userRepositoryMock.Setup(m => m.GetUserById(otherUserId)).ReturnsAsync((UserModel?)null);

        // Assert
        await Assert.ThrowsAsync<RestException>(() => _messageService.GetMessages(currentUserId, otherUserId));
    }

    [Fact]
    public async Task GetMessages_ValidScenario_ReturnsMessageList()
    {
        // Arrange
        const int currentUserId = 1;
        const int otherUserId = 2;
        var expectedMessages = new List<Message> {
            new(),
            new()
        };
        _userRepositoryMock.Setup(m => m.GetUserById(otherUserId)).ReturnsAsync(new UserModel());
        _messageRepositoryMock.Setup(m => m.GetMessages(currentUserId, otherUserId)).ReturnsAsync(expectedMessages);

        // Act
        var resultMessages = await _messageService.GetMessages(currentUserId, otherUserId);

        // Assert
        Assert.Equal(expectedMessages, resultMessages);
    }

    [Fact]
    public async Task SendMessage_ReceiverUserNotFound_ThrowsRestException()
    {
        // Arrange
        const int senderId = 1;
        const int receiverId = 2;
        const string content = "Hello world!";
        _userRepositoryMock.Setup(m => m.GetUserById(receiverId)).ReturnsAsync((UserModel?)null);

        // Assert
        await Assert.ThrowsAsync<RestException>(() => _messageService.SendMessage(senderId, receiverId, content));
    }

    [Fact]
    public async Task SendMessage_ValidScenario_SendsMessage()
    {
        // Arrange
        const int senderId = 1;
        const int receiverId = 2;
        const string content = "Hello world!";
        _userRepositoryMock.Setup(m => m.GetUserById(receiverId)).ReturnsAsync(new UserModel());

        // Act
        await _messageService.SendMessage(senderId, receiverId, content);

        // Assert
        _messageRepositoryMock.Verify(m => m.SendMessage(senderId, receiverId, content), Times.Once);
    }
}