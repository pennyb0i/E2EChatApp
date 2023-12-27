using E2EChatApp.Core.Domain.Models;

namespace E2EChatApp.Core.Application.Interfaces;

public interface IMessageService
{
    public Task<List<Message>> GetMessages(string user1Id, string user2Id);
}