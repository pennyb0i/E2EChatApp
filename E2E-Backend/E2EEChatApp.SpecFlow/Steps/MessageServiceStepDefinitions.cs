using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Application.Services;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;
using Moq;
namespace E2EEChatApp.SpecFlow.Steps;

[Binding]
public class MessageServiceStepDefinitions {
    private readonly IMessageService _messageService;

    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IMessageRepository> _messageRepository;
    
    public MessageServiceStepDefinitions()
    {
        _userRepository = new Mock<IUserRepository>();
        _messageRepository = new Mock<IMessageRepository>();
        _messageService = new MessageService(_messageRepository.Object, _userRepository.Object);
    }
    
    [BeforeScenario]
    public void SetupMocks()
    {
        // Inject Users 
        List<UserModel> users = new ()
        {
            new UserModel { Id=1, Username= "Bo_Benson" , Email = "BB@mail.com"}
        };
        
        _userRepository.Setup(x => x.GetUserById(1)).ReturnsAsync(users[0]);
        
    }

    #region Arrange
    
    [Given(@"the other user exists")]
    public void GivenTheOtherUserExists()
    {
        _userRepository.Setup(x => x.GetUserById(2)).ReturnsAsync(
            new UserModel { Id=2, Username= "Joe_Johnson" , Email = "JoJo@mail.com"});
    }

    [Given(@"the other user doesn't exist")]
    public void GivenTheOtherUserDoesntExist()
    {
        _userRepository.Setup(x => x.GetUserById(2)).ReturnsAsync((UserModel?)null);
    }

    #endregion

    #region Act
    
    [When(@"I send a message to the other user")]
    public void WhenISendAMessageToTheOtherUser()
    {
        _messageService.SendMessage(1, 2, "Hello world!");
    }
    
    [When(@"I fetch all messaging history with the other user")]
    public void WhenIFetchAllMessagingHistoryWithTheOtherUser()
    {
        _messageService.GetMessages(1, 2);
    }
    
    #endregion

    #region Assert
    
    [Then(@"the message is sent")]
    public void ThenTheMessageIsSent()
    {
        _messageRepository.Verify(repo => 
            repo.SendMessage(It.IsAny<int>(),It.IsAny<int>(),It.IsAny<string>()),Times.Exactly(1));
    }
    
    [Then(@"the messaging history is returned")]
    public void ThenTheMessagingHistoryIsReturned()
    {
        _messageRepository.Verify(repo => 
            repo.GetMessages(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(1));
    }
    
    [Then(@"an exception is thrown")]
    public void ThenAnExceptionIsThrown()
    {
        _messageRepository.Verify(repo => 
            repo.GetMessages(It.IsAny<int>(),It.IsAny<int>()),Times.Exactly(0));
        _messageRepository.Verify(repo => 
            repo.SendMessage(It.IsAny<int>(),It.IsAny<int>(),It.IsAny<string>()),Times.Exactly(0));
    }
    
    #endregion
}