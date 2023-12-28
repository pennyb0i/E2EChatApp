using E2EChatApp.Core.Domain.BindingModels;
using E2EChatApp.Core.Domain.Dtos;
using E2EChatApp.Core.Domain.Models;
namespace E2EChatApp.Core.Domain.Interfaces;

public interface IUserRepository {

    /// <summary>
    /// Get all users in the system
    /// </summary>
    /// <param name="friendsOnly"></param>
    /// <param name="currentUserId"></param>
    /// <returns></returns>
    public Task<List<UserDto>> GetAllUsers(bool? friendsOnly, int currentUserId);
    
    /// <summary>
    /// Get a user by primary key Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<UserModel?> GetUserById(int id);
    
    /// <summary>
    /// Get a user by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public Task<UserModel?> GetUserByEmail(string email);
    
    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<int?> CreateUser(UserPostBindingModel model);
}
