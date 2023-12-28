using E2EChatApp.Core.Domain.Models;

namespace E2EChatApp.Core.Application.Interfaces;

public interface IMessageService
{
    public Task<List<Message>> GetMessages(int firstUserId, int secondUserId);

    Task SendMessage(int senderId, int receiverId, string content);
}