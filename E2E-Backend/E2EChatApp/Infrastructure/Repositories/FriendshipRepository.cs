using Dapper;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Infrastructure.Factories;
namespace E2EChatApp.Infrastructure.Repositories;

public class FriendshipRepository : IFriendshipRepository{
    private readonly IDbConnectionFactory _connectionFactory;

    public FriendshipRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task<bool> CreateFriendship(int senderId, int receiverId)
    {
        using var conn = await _connectionFactory.CreateAsync();
        const string query = 
            """
                INSERT INTO friendships (sender_id, receiver_id)
                VALUES (@senderId,@receiverId)
            """;
        var rowsAffected = await conn.ExecuteAsync(query,new {senderId,receiverId});
        return rowsAffected == 1;
    }
    
    public async Task<bool> ConfirmFriendship(int senderId, int receiverId)
    {
        using var conn = await _connectionFactory.CreateAsync();
        const string query = 
            """
                UPDATE friendships SET
                    is_pending = false
                WHERE sender_id = @senderId
                  AND receiver_id = @receiverId
            """;
        var rowsAffected = await conn.ExecuteAsync(query,new {senderId,receiverId});
        return rowsAffected == 1;
    }
    
    public async Task<bool> CancelFriendship(int senderId, int receiverId)
    {
        using var conn = await _connectionFactory.CreateAsync();
        const string query = 
            """
                DELETE FROM friendships 
                WHERE sender_id = @senderId
                  AND receiver_id = @receiverId
            """;
        var rowsAffected = await conn.ExecuteAsync(query,new {senderId,receiverId});
        return rowsAffected == 1;
    }
}
