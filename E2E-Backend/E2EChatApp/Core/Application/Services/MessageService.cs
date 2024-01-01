using System.Net;
using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Domain.Exceptions;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;

namespace E2EChatApp.Core.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public MessageService(IMessageRepository messageRepository, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }
    
    public async Task<List<Message>> GetMessages(int currentUserId, int otherUserId)
    {
        if (await _userRepository.GetUserById(otherUserId) is null) {
            throw new RestException(HttpStatusCode.NotFound, $"User with id {otherUserId} not found!");
        }
        return await _messageRepository.GetMessages(currentUserId, otherUserId);
    }
    
    public async Task SendMessage(int senderId, int receiverId, string content)
    {
        if (await _userRepository.GetUserById(receiverId) is null) {
            throw new RestException(HttpStatusCode.NotFound, $"User with id {receiverId} not found!");
        }
        await _messageRepository.SendMessage(senderId, receiverId, content);
    }
}