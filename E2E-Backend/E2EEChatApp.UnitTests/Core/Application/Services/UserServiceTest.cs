using E2EChatApp.Core.Application.Interfaces;
using Moq;
using E2EChatApp.Core.Application.Services;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;
using E2EChatApp.Core.Domain.Dtos;
using JetBrains.Annotations;

namespace E2EEChatApp.UnitTests.Core.Application.Services;

[TestSubject(typeof(UserService))]
public class UserServiceTest {
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly IUserService _userService;

    public UserServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnCorrectResult()
    {
        // Arrange
        var allUsers = new List<UserDto> {
            new() {
                Id = 1,
                Email = "test1@example.com",
                Username = "test1"
            },
            new() {
                Id = 2,
                Email = "test2@example.com",
                Username = "test2"
            }
        };
        _userRepositoryMock.Setup(x => x.GetAllUsers(It.IsAny<bool?>(), It.IsAny<int>())).ReturnsAsync(allUsers);

        // Act
        var result = await _userService.GetAllUsers(true, 1);

        // Assert
        Assert.Equal(allUsers, result);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnCorrectResult()
    {
        // Arrange
        var user = new UserModel {
            Id = 1,
            Email = "test1@example.com",
            Username = "Test1"
        };
        _userRepositoryMock.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserById(user.Id);

        // Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task GetUsers_ShouldReturnCorrectResult()
    {
        // Arrange
        var users = new List<UserModel> {
            new() {
                Id = 1,
                Email = "test1@example.com",
                Username = "Test1"
            },
            new() {
                Id = 2,
                Email = "test2@example.com",
                Username = "Test2"
            }
        };
        _userRepositoryMock.Setup(x => x.GetUsers()).ReturnsAsync(users);

        // Act
        var result = await _userService.GetUsers();

        // Assert
        Assert.Equal(users, result);
    }
}