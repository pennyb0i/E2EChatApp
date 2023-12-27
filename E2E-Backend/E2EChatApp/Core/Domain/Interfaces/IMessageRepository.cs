using E2EChatApp.Core.Domain.Models;

namespace E2EChatApp.Core.Domain.Interfaces;

public interface IMessageRepository
{
    Task<List<Message>> GetMessages(int user1Id, int user2Id);
    Task SendMessage(int senderId, int receiverId, string content);
}