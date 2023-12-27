using E2EChatApp.Core.Domain.Models;

namespace E2EChatApp.Core.Domain.Interfaces;

public interface IMessageRepository
{
    Task<List<Message>> GetMessages(string user1Id, string user2Id);

}