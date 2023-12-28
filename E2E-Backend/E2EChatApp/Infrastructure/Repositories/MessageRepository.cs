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
    
            const string query = 
                """
                    SELECT *
                     FROM messages
                    WHERE (sender_id = @user1Id AND receiver_id = @user2Id)
                       OR (sender_id = @user2Id AND receiver_id = @user1Id)
                    ORDER BY Timestamp
                """;

            var messages = await conn.QueryAsync<Message>(query, new { user1Id, user2Id });
            return messages.ToList();
        }
        
        public async Task SendMessage(int senderId, int receiverId, string content)
        {
            using var conn = await _connectionFactory.CreateAsync();

            const string query = 
                """                 
                    INSERT INTO messages (sender_id, receiver_id, Content, Timestamp)
                    VALUES (@senderId, @receiverId, @content, NOW())
                """;

            await conn.ExecuteAsync(query, new { senderId, receiverId, content });
        }
    }
}