using Dapper;
using E2EChatApp.Core.Domain.BindingModels;
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
    
    public async Task<User?> GetUserById(int id)
    {
        using var conn = await _connectionFactory.CreateAsync();
        const string query = "SELECT * FROM Users WHERE id = @id";
        var user = await conn.QueryFirstOrDefaultAsync<User>(query, new {
            id
        });
        return user;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        using var conn = await _connectionFactory.CreateAsync();
        const string query = "SELECT * FROM Users WHERE Email = @email";
        var user = await conn.QueryFirstOrDefaultAsync<User>(query, new {
            email
        });
        return user;
    }
    
    #endregion

    #region INSERT
    public async Task<int?> CreateUser(UserPostBindingModel model)
    {
        using var conn = await _connectionFactory.CreateAsync();
        const string query = 
            """
                INSERT INTO Users ( Email, PasswordHash, PasswordSalt )
                VALUES ( @Email, @PasswordHash, @PasswordSalt )
                RETURNING ID;
            """;
        var userId = await conn.QueryFirstOrDefaultAsync<int?>(query, model);
        return userId;
    }
    
    #endregion
}
