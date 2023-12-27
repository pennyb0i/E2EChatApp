using E2EChatApp.Core.Domain.BindingModels;
using E2EChatApp.Core.Domain.Models;
namespace E2EChatApp.Core.Domain.Interfaces;

public interface IUserRepository {
    /// <summary>
    /// Get a user by primary key Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<User?> GetUserById(int id);
    
    /// <summary>
    /// Get a user by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public Task<User?> GetUserByEmail(string email);
    
    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<int?> CreateUser(UserPostBindingModel model);
    Task<List<User>> GetUsers();
}
