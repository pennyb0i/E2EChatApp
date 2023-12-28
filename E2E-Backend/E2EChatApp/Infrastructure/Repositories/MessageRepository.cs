using Dapper;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;
using E2EChatApp.Infrastructure.Factories;

namespace E2EChatApp.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public MessageRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Message>> GetMessages(int user1Id, int user2Id)
        {
            using var conn = await _connectionFactory.CreateAsync();
    
            const string query = @"
        SELECT *
        FROM messages
        WHERE (SenderId = @user1Id AND ReceiverId = @user2Id)
           OR (SenderId = @user2Id AND ReceiverId = @user1Id)
        ORDER BY Timestamp ASC";

            var messages = await conn.QueryAsync<Message>(query, new { user1Id, user2Id });
            return messages.ToList();
        }
        
        public async Task SendMessage(int senderId, int receiverId, string content)
        {
            using var conn = await _connectionFactory.CreateAsync();

            const string query = @"
                INSERT INTO messages (SenderId, ReceiverId, Content, Timestamp)
                VALUES (@senderId, @receiverId, @content, NOW())";

            await conn.ExecuteAsync(query, new { senderId, receiverId, content });
        }
    }
}