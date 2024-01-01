using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Application.Services;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;
using Moq;
namespace E2EEChatApp.SpecFlow.Steps;

[Binding]
public class FriendServiceStepDefinitions {
    private readonly IFriendshipService _friendshipService;
    
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IFriendshipRepository> _friendshipRepository;
    public FriendServiceStepDefinitions()
    {
        _userRepository = new Mock<IUserRepository>();
        _friendshipRepository = new Mock<IFriendshipRepository>();
        _friendshipService = new FriendshipService(_friendshipRepository.Object, _userRepository.Object);
    }
    
    [BeforeScenario]
    public void SetupMocks()
    {
        // Inject Users 
        List<UserModel> users = new ()
        {
            new UserModel { Id=1, Username= "Bo_Benson" , Email = "BB@mail.com"},
            new UserModel { Id=2, Username= "Joe_Johnson" , Email = "JoJo@mail.com"}
        };
        
        _userRepository.Setup(x => x.GetUserById(1)).ReturnsAsync(users[0]);
        _userRepository.Setup(x => x.GetUserById(2)).ReturnsAsync(users[1]);
    }
    
    [Given("there (.*) a friendship between me and the other user, initiated by (.*), and the friendship (.*) pending")]
    public void GivenThereAFriendshipBetweenMeAndTheOtherUserInitiatedByAndTheFriendshipPending(bool? friendshipExists, int? senderUserId, bool? isPending)
    {
        if (friendshipExists != true) return;
        if (senderUserId is null) {
            throw new ArgumentException("Unknown user specified");
        }
        if (isPending is null) {
            throw new ArgumentException("Pending status not specified");
        }
        var receiverUserId = senderUserId == 1 ? 2 : 1;
        var friendship = new FriendshipModel {
            SenderId = (int)senderUserId,
            ReceiverId = receiverUserId,
            IsPending = (bool)isPending
        };
        _friendshipRepository.Setup(x => x.GetFriendship((int)senderUserId, receiverUserId)).ReturnsAsync(
            friendship);
        _friendshipRepository.Setup(x => x.GetFriendship(receiverUserId,(int)senderUserId)).ReturnsAsync(
            friendship);
    }
    
    [When(@"(.*) sends a create request to (.*)")]
    public void WhenSendsACreateRequestTo(int? senderId, int? receiverId)
    {
        if (senderId is null || receiverId is null) {
            throw new ArgumentException("Unknown user specified");
        }
        _friendshipService.CreateFriendship((int)senderId, (int)receiverId);
    }
    
    [When(@"(.*) sends a cancel request to (.*)")]
    public void WhenMyUserSendsACancelRequestToOtherUser(int? senderId, int? receiverId)
    {
        if (senderId is null || receiverId is null) {
            throw new ArgumentException("Unknown user specified");
        }
        _friendshipService.CancelFriendship((int)senderId, (int)receiverId);
    }
    
    [Then(@"the result is: (.*)")]
    public void ThenTheResultIs(Result result)
    {
        switch (result) {
            // Create is called exactly once
            case Result.Created:
                _friendshipRepository.Verify(repo => 
                    repo.CreateFriendship(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(1));
                _friendshipRepository.Verify(repo => 
                    repo.ConfirmFriendship(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(0));
                _friendshipRepository.Verify(repo => 
                    repo.CancelFriendship(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(0));
                break;
            // Confirm is called exactly once
            case Result.Accepted:
                _friendshipRepository.Verify(repo => 
                    repo.CreateFriendship(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(0));
                _friendshipRepository.Verify(repo => 
                    repo.ConfirmFriendship(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(1));
                _friendshipRepository.Verify(repo => 
                    repo.CancelFriendship(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(0));
                break;
            // Nothing is ever called
            case Result.Nothing:
                _friendshipRepository.Verify(repo => 
                    repo.CreateFriendship(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(0));
                _friendshipRepository.Verify(repo => 
                    repo.ConfirmFriendship(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(0));
                _friendshipRepository.Verify(repo => 
                    repo.CancelFriendship(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(0));
                break;
            case Result.Cancelled:
                _friendshipRepository.Verify(repo => 
                    repo.CreateFriendship(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(0));
                _friendshipRepository.Verify(repo => 
                    repo.ConfirmFriendship(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(0));
                _friendshipRepository.Verify(repo => 
                    repo.CancelFriendship(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(1));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(result), result, null);
        }
    }

    #region StepArgumentTransformations
    
    [StepArgumentTransformation]
    public bool? IsExpressionToBoolean(string isExpression)
    {
        return isExpression switch {
            "is" => true,
            "is not" => false,
            _ => null
        };
    }
    
    [StepArgumentTransformation]
    public int? UserExpressionToUserId(string userExpression)
    {
        return userExpression switch {
            "my user" => 1,
            "other user" => 2,
            _ => null
        };
    }
    
    #endregion
}

public enum Result
{
    Created,
    Accepted,
    Nothing,
    Cancelled
}