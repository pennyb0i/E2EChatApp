using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Application.Services;
using E2EChatApp.Core.Domain.Dtos;
using E2EChatApp.Core.Domain.Enums;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;
using Moq;
using Xunit;

namespace E2EChatAppSpecFlow.Steps;

[Binding]
public sealed class UserStepDefinition
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly IUserService _userService;
    private int _userId;
    private Task<UserModel?> _user;
    private Task<List<UserDto>> _users;
    private bool _friendsOnly;

    public UserStepDefinition()
    {
        _userRepository = new Mock<IUserRepository>();
        _userService = new UserService(_userRepository.Object);
    }
    
    #region Setup
    [BeforeScenario]
    public void SetupMocks()
    {
         
        List<UserDto> usersDtos = new ()
        {
            new UserDto { Id=1,  Email = "BB@mail.com",PublicKey = "asd123",FriendshipStatus = FriendshipStatus.Friend},
            new UserDto { Id=2,  Email = "JoJo@mail.com",PublicKey = "asd123", FriendshipStatus = FriendshipStatus.NotFriend},
            new UserDto { Id=3,  Email = "JoJoPending@mail.com",PublicKey = "asd123", FriendshipStatus = FriendshipStatus.PendingSent},
            new UserDto { Id=4,  Email = "JoJoReceive@mail.com",PublicKey = "asd123", FriendshipStatus = FriendshipStatus.PendingReceiving},
            new UserDto { Id=5,  Email = "JoJoFriend@mail.com",PublicKey = "asd123", FriendshipStatus = FriendshipStatus.Friend},
        };
        
        List<UserModel> usersModels = new ()
        {
            new UserModel { Id = 1, Username = "Bo_Benson", Email = "BB@mail.com" },
            new UserModel { Id = 2, Username = "Joe_Johnson", Email = "JoJo@mail.com" },
        };

        _userRepository.Setup(x => x.GetUserById(1)).ReturnsAsync(usersModels[0]);
        _userRepository.Setup(x => x.GetUserById(2)).ReturnsAsync(usersModels[1]);
        _userRepository.Setup(x => x.GetAllUsers(true,1)).ReturnsAsync(new List<UserDto>()
        {
            new UserDto { Id=5,  Email = "JoJoFriend@mail.com",PublicKey = "asd123", FriendshipStatus = FriendshipStatus.Friend},
        });
        _userRepository.Setup(x => x.GetAllUsers(false,1)).ReturnsAsync(usersDtos);
        _userRepository.Setup(x => x.GetAllUsers(true,2)).ReturnsAsync(new List<UserDto>()
        {
            new UserDto { Id=1,  Email = "BB@mail.com",PublicKey = "asd123",FriendshipStatus = FriendshipStatus.Friend},
            new UserDto { Id=5,  Email = "JoJoFriend@mail.com",PublicKey = "asd123", FriendshipStatus = FriendshipStatus.Friend}
        });
        _userRepository.Setup(x => x.GetAllUsers(false,2)).ReturnsAsync(usersDtos);
        
    }
    #endregion

    #region GetAllUser
    [Given(@"I am a logged-in user with ID ""(.*)""")]
    public void GivenIAmALoggedInUserWithId(int currentUserId)
    {
        _userId= currentUserId;
    }

    [When(@"I request all users with friendsOnly set to ""(.*)""")]
    public void WhenIRequestAllUsersWithFriendsOnlySetTo(bool friendsOnly)
    {
        _users = _userRepository.Object.GetAllUsers(friendsOnly, _userId);
        _friendsOnly = friendsOnly;
    }

    [Then(@"I should receive a list of users")]
    public void ThenIShouldReceiveAListOfUsers()
    {
        IEnumerable<UserDto> expectedData = new List<UserDto>();
        if (_friendsOnly)
        {
            if (_userId == 1)
            {
                expectedData = new List<UserDto>
                {
                    new() { Id=5,  Email = "JoJoFriend@mail.com",PublicKey = "asd123", FriendshipStatus = FriendshipStatus.Friend}
                };
            }
            else if (_userId == 2)
            {
                expectedData = new List<UserDto>
                {
                    new() { Id=1,  Email = "BB@mail.com",PublicKey = "asd123",FriendshipStatus = FriendshipStatus.Friend},
                    new() { Id=5,  Email = "JoJoFriend@mail.com",PublicKey = "asd123", FriendshipStatus = FriendshipStatus.Friend}
                }; 
            }
             
        }
        else
        {
             expectedData = new List<UserDto>
            {
                new() { Id=1,  Email = "BB@mail.com",PublicKey = "asd123",FriendshipStatus = FriendshipStatus.Friend},
                new() { Id=2,  Email = "JoJo@mail.com",PublicKey = "asd123", FriendshipStatus = FriendshipStatus.NotFriend},
                new() { Id=3,  Email = "JoJoPending@mail.com",PublicKey = "asd123", FriendshipStatus = FriendshipStatus.PendingSent},
                new() { Id=4,  Email = "JoJoReceive@mail.com",PublicKey = "asd123", FriendshipStatus = FriendshipStatus.PendingReceiving},
                new() { Id=5,  Email = "JoJoFriend@mail.com",PublicKey = "asd123", FriendshipStatus = FriendshipStatus.Friend},
            };
        }
        
        Assert.Equal(expectedData, _users.Result, EqualityComparer<UserDto>.Default);
    }
    #endregion

    #region GetUserById
    
    [Given(@"I have a user ID ""(.*)""")]
    public void GivenIHaveAUserId(int userId)
    {
        _userId = userId;
    }
    
    [When(@"I request user details for that ID")]
    public void WhenIRequestUserDetailsForThatId()
    {
        _user = _userRepository.Object.GetUserById(_userId);
    }
    
    [Then(@"I should receive the user details")]
    public void ThenIShouldReceiveTheUserDetails()
    {
        
        var expectedUserDetails = _userId switch
        {
            1 => new UserModel {Id = 1, Username = "Bo_Benson", Email = "BB@mail.com"},
            2 => new UserModel {Id = 2, Username = "Joe_Johnson", Email = "JoJo@mail.com"},
            _ => null
        };

        Assert.Equal(expectedUserDetails?.Id, _user?.Result?.Id);
        Assert.Equal(expectedUserDetails?.Username, _user?.Result?.Username);
        Assert.Equal(expectedUserDetails?.Email, _user?.Result?.Email);
        
    }
    #endregion

    
}