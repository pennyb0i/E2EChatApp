using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;

namespace E2EChatApp.Core.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;

    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    public async Task<List<Message>> GetMessages(int firstUserId, int secondUserId)
    {
        return await _messageRepository.GetMessages(firstUserId, secondUserId);
    }
    
    public async Task SendMessage(int senderId, int receiverId, string content)
    {
        await _messageRepository.SendMessage(senderId, receiverId, content);
    }
}