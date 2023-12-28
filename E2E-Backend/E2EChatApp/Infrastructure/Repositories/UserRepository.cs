using Dapper;
using E2EChatApp.Core.Domain.BindingModels;
using E2EChatApp.Core.Domain.Dtos;
using E2EChatApp.Core.Domain.Enums;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;
using E2EChatApp.Infrastructure.Factories;
namespace E2EChatApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    #region SELECT
    public async Task<List<UserDto>> GetAllUsers(bool? friendsOnly, int currentUserId)
    {
        using var conn = await _connectionFactory.CreateAsync();
        const string query = 
            """
                SELECT us.*, 
                    (CASE 
                        WHEN fr.is_pending = TRUE AND fr.sender_id = @id THEN @pendingSent
                        WHEN fr.is_pending = TRUE AND fr.receiver_id = @id THEN @pendingReceiving
                        WHEN fr.is_pending = FALSE THEN @friend
                        ELSE @notFriend
                    END) AS friendship_status
                FROM users us
                  LEFT JOIN friendships fr ON fr.sender_id = us.id OR fr.receiver_id = us.id
                  WHERE us.id <> @id
                    AND ((@friendsOnly IS NULL) 
                     OR (@friendsOnly = TRUE AND fr.is_pending = FALSE)
                     OR (@friendsOnly = FALSE AND (fr.is_pending is NULL OR fr.is_pending = TRUE)))
            """;
        var users = await conn.QueryAsync<UserDto>(query, new {
            id = currentUserId,
            friendsOnly,
            FriendshipStatus.NotFriend,
            FriendshipStatus.PendingSent,
            FriendshipStatus.PendingReceiving,
            FriendshipStatus.Friend
        });
        return users.ToList();
    }
    
    public async Task<UserModel?> GetUserById(int id)
    {
        using var conn = await _connectionFactory.CreateAsync();
        const string query = "SELECT * FROM users WHERE id = @id";
        var user = await conn.QueryFirstOrDefaultAsync<UserModel>(query, new {
            id
        });
        return user;
    }

    public async Task<UserModel?> GetUserByEmail(string email)
    {
        using var conn = await _connectionFactory.CreateAsync();
        const string query = "SELECT * FROM users WHERE Email = @email";
        var user = await conn.QueryFirstOrDefaultAsync<UserModel>(query, new {
            email
        });
        return user;
    }
    
    public async Task<List<User>> GetUsers()
    {
        using var conn = await _connectionFactory.CreateAsync();
    
        const string query = "SELECT *FROM users";

        var users = await conn.QueryAsync<User>(query);
        return users.ToList();
    }
    
    #endregion

    #region INSERT
    public async Task<int?> CreateUser(UserPostBindingModel model)
    {
        using var conn = await _connectionFactory.CreateAsync();
        const string query = 
            """
                INSERT INTO users ( Email, Password_Hash, Password_Salt, Username,Public_Key )
                VALUES ( @Email, @PasswordHash, @PasswordSalt, @Username, @PublicKey )
                RETURNING ID;
            """;
        var userId = await conn.QueryFirstOrDefaultAsync<int?>(query, model);
        return userId;
    }

    #endregion
}
