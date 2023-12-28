using Dapper;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;
using E2EChatApp.Infrastructure.Factories;
namespace E2EChatApp.Infrastructure.Repositories;

public class FriendshipRepository : IFriendshipRepository{
    private readonly IDbConnectionFactory _connectionFactory;

    public FriendshipRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public async Task<FriendshipModel?> GetFriendship(int senderId, int receiverId)
    {
        using var conn = await _connectionFactory.CreateAsync();
        const string query = 
            """
                SELECT * FROM friendships fr
                    JOIN users Sender ON Sender.id = fr.sender_id
                    JOIN users Receiver ON Receiver.id = fr.receiver_id
                    WHERE sender_id IN (@senderId, @receiverId)
                      AND receiver_id IN (@senderId, @receiverId)
            """;
        var friendship = await conn.QueryAsync<FriendshipModel,UserModel,UserModel,FriendshipModel>(
            query,
            (friendship, sender, receiver) =>
            {
                friendship.Sender = sender;
                friendship.Receiver = receiver;
                return friendship;
            },
            new {senderId,receiverId}
            );
        return friendship.FirstOrDefault();
    }

    public async Task<List<FriendshipModel>> GetFriendshipsByUserId(int userId)
    {
        using var conn = await _connectionFactory.CreateAsync();
        const string query = 
            """
                SELECT * FROM friendships fr
                    JOIN users Sender ON Sender.id = fr.sender_id
                    JOIN users Receiver ON Receiver.id = fr.receiver_id
                    WHERE sender_id = @userId
                      OR receiver_id = @userId
            """;
        var friendships = await conn.QueryAsync<FriendshipModel,UserModel,UserModel,FriendshipModel>(
            query,
            (friendship, sender, receiver) =>
            {
                friendship.Sender = sender;
                friendship.Receiver = receiver;
                return friendship;
            },
            new {userId}
        );        
        return friendships.ToList();
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
