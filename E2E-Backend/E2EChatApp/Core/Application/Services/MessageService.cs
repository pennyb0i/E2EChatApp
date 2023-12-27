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
    public async Task<List<Message>> GetMessages(string user1Id, string user2Id)
    {
        return await _messageRepository.GetMessages(user1Id, user2Id);
    }
}